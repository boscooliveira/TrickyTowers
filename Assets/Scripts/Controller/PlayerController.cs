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

        public PlayerAreaBoundaries Bounds => _bounds;

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
            SetupPlayer(playerData);
            DisplayLife();
        }

        public float GetPileHeight()
        {
            if (_placedPieces.Count == 0)
                return _bounds.LimitBottom.position.y;

            var maxY = _placedPieces.Max(i => ((PieceController)i).gameObject.transform.position.y);
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
                    break;

                case EPlayerInputType.RandomTargetAI:
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
            _nextPiece.gameObject.SetActive(false);

            if (_nextPieceRenderer != null)
            {
                _nextPieceRenderer.sprite = _nextPiece.SpriteRenderer.sprite;
            }

            _currentPiece = piece;
            _currentPiece.gameObject.SetActive(true);
            piece.OnMoveFinished += OnPieceMoveFinished;
            piece.OnDisabled += PieceLost;
            _pieceHighlight.gameObject.SetActive(true);
            UpdateHighlight();
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

        private void PieceLost(IPoolableItem obj)
        {
            _placedPieces.Remove(obj);
            GetHit();

            if (_playerData.Lives == 0)
            {
                GameOver();
            }
            else
            {
                OnPieceMoveFinished(obj);
            }
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

        private void OnPieceMoveFinished(IPoolableItem controller)
        {
            if (_playerData.Lives == 0)
                return;

            if (_currentPiece == (object)controller)
            {
                _placedPieces.Add(_currentPiece);
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