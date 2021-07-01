using GameProject.TrickyTowers.Engine;
using GameProject.TrickyTowers.Model;
using GameProject.TrickyTowers.Config;
using GameProject.TrickyTowers.Utils;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
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

        private IPieceFactoryConfig _config;
        private float _ghostTime;
        private IGameplayConfig _gameplayConfig;

        private PlayerData _playerData;
        private GameController.OnGameOverDelegate _onGameOver;

        public void Initialize(IPieceFactoryConfig config, IPhysicsConfig physicsConfig,
Service.IGameplayService gameplayService, PlayerData playerData, GameController.OnGameOverDelegate onGameOver)
        {
            _onGameOver = onGameOver;
            _config = config;
            _gameplayConfig = gameplayService.GetGameData().Config;
            _pieceFactory = new PieceFactory(config, _bounds, physicsConfig);
            SetupPlayer(playerData);
            DisplayLife();
        }

        public void SetSmallCameraProperties()
        {
            _camera.targetTexture = _renderTexture;
        }

        private void SetupPlayer(PlayerData playerData)
        {
            _playerData = playerData;
            switch (playerData.Input)
            {
                case EPlayerInputType.Player1:
                    _input.enabled = true;
                    break;

                case EPlayerInputType.RandomTargetAI:
                    _aiInput.enabled = true;                    
                    var aiLogic = new RandomTargetAIAlgorithm(
                        _baseMinPosX.transform.position.x, 
                        _baseMaxPosX.transform.position.x);
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
            UpdateHighLight();
        }

        public PieceController GetPiece()
        {
            return _currentPiece;
        }

        private void CreateNewPiece()
        {
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
            UpdateHighLight();
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

        private void UpdateHighLight()
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
            UpdateHighLight();
        }

        private void Update()
        {
            _ghostTime -= Time.deltaTime;
        }
    }
}