using GameProject.TrickyTowers.Engine;
using GameProject.TrickyTowers.Model;
using GameProject.TrickyTowers.Config;
using GameProject.TrickyTowers.Utils;
using UnityEngine;
using System;

namespace GameProject.TrickyTowers.Controller
{
    public class PlayerController : MonoBehaviour
    {
        public event Action<PieceController> OnPieceChanged;

        [SerializeField]
        private PlayerAreaBoundaries _bounds;

        [SerializeField]
        private Transform _pieceHighlight;

        [SerializeField]
        private InputHandler _input;

        private PieceFactory _pieceFactory;

        private PieceController _currentPiece;

        private IPieceFactoryConfig _config;
        private float _pace;

        public void Initialize(IPieceFactoryConfig config, IPhysicsConfig physicsConfig)
        {
            _config = config;
            _pace = _config.SlowPace;
            _pieceFactory = new PieceFactory(config, _bounds, physicsConfig);
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

        private void CreateNewPiece()
        {
            var piece = _pieceFactory.GetPiece();
            _currentPiece = piece;
            piece.OnMoveFinished += OnPieceMoveFinished;
            piece.OnDisabled += OnPieceMoveFinished;
            _pieceHighlight.gameObject.SetActive(true);
            UpdateHighLight();
            OnPieceChanged?.Invoke(piece);
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
    }
}