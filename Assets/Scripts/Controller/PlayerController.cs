using GameProject.TrickyTowers.Engine;
using GameProject.TrickyTowers.Model;
using GameProject.TrickyTowers.Config;
using GameProject.TrickyTowers.Utils;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using System.Collections.Generic;
using GameProject.TrickyTowers.Model.AIAlgorithm;

namespace GameProject.TrickyTowers.Controller
{
    public class PlayerController : MonoBehaviour
    {
        public event Action<PieceController> OnPieceChanged;

        [SerializeField]
        private Camera _camera;

        [SerializeField]
        private RenderTexture _renderTexture;

        [SerializeField]
        private PlayerAreaBoundaries _bounds;

        [SerializeField]
        private Transform _pieceHighlight;

        [SerializeField]
        private InputHandler _input;

        [SerializeField]
        private AIController _aiInput;

        [SerializeField]
        private Transform _baseMinPosX;

        [SerializeField]
        private Transform _baseMaxPosX;
        [SerializeField]
        private Image _nextPieceRenderer;

        [SerializeField]
        private TextMeshProUGUI _livesText;

        private PieceFactory _pieceFactory;
        private PieceController _currentPiece;
        private PieceController _nextPiece;
        private HashSet<IPoolableItem> _placedPieces;
        private IPieceFactoryConfig _config;
        private float _ghostTime;
        private IGameplayConfig _gameplayConfig;
        private IPlayerData _playerData;
        private GameController.OnGameOverDelegate _onGameOver;

        public void Initialize(IPieceFactoryConfig pieceFactoryConfig, IPhysicsConfig physicsConfig,
Service.IGameplayService gameplayService, IPlayerData playerData, GameController.OnGameOverDelegate onGameOver)
        {
            _placedPieces = new HashSet<IPoolableItem>();
            _onGameOver = onGameOver;
            _config = pieceFactoryConfig;
            _gameplayConfig = gameplayService.GetGameData().Config;
            _pieceFactory = new PieceFactory(pieceFactoryConfig, _bounds, physicsConfig);
            UpdatePositions(_gameplayConfig.GoalHeight, _gameplayConfig.SpawnerHeight);
            SetupPlayer(playerData);
            DisplayLife();
        }

        public float GetPileHeight()
        {
            if (_placedPieces.Count == 0)
                return _bounds.LimitBottom.position.y;

            var maxY = _placedPieces.Max(i => ((PieceController)i).GetPieceHeight());
            return Mathf.Max(maxY, _bounds.LimitBottom.position.y);
        }

        public void SetSmallCameraProperties()
        {
            _camera.targetTexture = _renderTexture;
        }

        private void SetupPlayer(IPlayerData playerData)
        {
            _playerData = playerData;
            switch (playerData.Input)
            {
                case EPlayerInputType.Player1:
                    _input.enabled = true;
                    _aiInput.enabled = false;
                    break;

                case EPlayerInputType.RandomTargetAI:
                    _input.enabled = false;
                    _aiInput.enabled = true;
                    var aiLogic = new RandomTargetAI(_baseMinPosX.transform.position.x, _baseMaxPosX.transform.position.x);
                    _aiInput.SetAlgorithm(aiLogic);
                    break;

                default:
                    Debug.LogError("Unsupported input type");
                    break;
            }
        }

        private void Awake()
        {
            _pieceHighlight.gameObject.SetActive(false);
        }

        private void Start()
        {
            CreateNewPiece();
        }

        public void RotatePiece()
        {
            if (_currentPiece == null)
                return;

            _currentPiece.Rotate();
            UpdateHighlight();
        }

        public PieceController GetPiece()
        {
            return _currentPiece;
        }

        private void CreateNewPiece()
        {
            if (GetPileHeight() > _bounds.Goal.position.y)
            {
                GameOver();
                return;
            }

            if (_nextPiece == null)
            {
                _nextPiece = _pieceFactory.GetPiece();
            }
            var piece = _nextPiece;
            _nextPiece = _pieceFactory.GetPiece();
            _nextPiece.ResetToDefault();
            _nextPiece.gameObject.SetActive(false);

            if (_nextPieceRenderer != null)
            {
                _nextPieceRenderer.sprite = _nextPiece.SpriteRenderer.sprite;
            }

            _currentPiece = piece;
            _currentPiece.ResetToDefault();
            _currentPiece.gameObject.SetActive(true);
            _currentPiece.OnMoveFinished += OnPlacedPiece;
            _currentPiece.OnDisabled += OnPieceLost;

            UpdateHighlight();
            _pieceHighlight.gameObject.SetActive(true);
            
            OnPieceChanged?.Invoke(piece);
        }

        private void GetHit()
        {
            if (_ghostTime > 0)
                return;

            _playerData.LoseLife();
            _ghostTime = _gameplayConfig.GhostSecsAfterHit;

            DisplayLife();
        }

        private void DisplayLife()
        {
            if (_livesText == null)
                return;
            _livesText.text = _playerData.Lives.ToString();
        }

        private void OnPieceLost(IPoolableItem obj)
        {
            _placedPieces.Remove(obj);
            GetHit();

            if (_playerData.Lives == 0)
            {
                GameOver();
            }
            else
            {
                MoveToNextPiece(obj);
            }
        }

        public void UpdatePositions(float goalHeight, float spawnerHeight)
        {
            var bottom = _bounds.LimitBottom.position;

            var position = _bounds.Goal.position;
            position.y = bottom.y + goalHeight;
            _bounds.Goal.position = position;

            position = _bounds.SpawnerPosition.position;
            position.y = bottom.y + spawnerHeight;
            _bounds.SpawnerPosition.position = position;
        }

        private void GameOver()
        {
            _onGameOver(_playerData, this);
            enabled = false;
        }

        private void UpdateHighlight()
        {
            var bounds = _currentPiece.GetBounds();
            var highlightTransform = _pieceHighlight.transform;
            highlightTransform.position = bounds.center;

            Vector3 scale = new Vector3
            {
                x = bounds.size.x / highlightTransform.parent.lossyScale.x,
                y = highlightTransform.localScale.y
            };
            highlightTransform.localScale = scale;
        }

        private void OnPlacedPiece(IPoolableItem controller)
        {
            _placedPieces.Add(_currentPiece);
            MoveToNextPiece(controller);
        }

        private void MoveToNextPiece(IPoolableItem controller)
        {
            if (_playerData.Lives == 0)
                return;

            if (_currentPiece == (object)controller)
            {
                _currentPiece.SetSpeed(_config.SlowPace);
                CreateNewPiece();
            }
        }

        public void MovePiece(Vector2 input)
        {
            if (_currentPiece == null)
                return;

            _currentPiece.Move(input);
            UpdateHighlight();
        }

        private void Update()
        {
            _ghostTime -= Time.deltaTime;
        }
    }
}