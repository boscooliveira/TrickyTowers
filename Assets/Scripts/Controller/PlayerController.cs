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
        [SerializeField]
        private PlayerAreaBoundaries _bounds;

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

        private void Start()
        {
            CreateNewPiece();
        }

        public void RotatePiece()
        {
            if (_currentPiece == null)
                return;

            _currentPiece.Rotate();
        }

        private void CreateNewPiece()
        {
            var piece = _pieceFactory.CreatePiece();
            _currentPiece = piece;
            piece.OnMoveFinished += OnPieceMoveFinished;
            piece.OnDisabled += OnPieceMoveFinished;
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
            
            var position = _currentPiece.transform.position;
            position.x += input.x * _config.HorizontalMoveDistance;
            if (input.y < 0)
            {
                if (_currentPiece.Pace != _config.FastPace)
                {
                    _currentPiece.SetSpeed(_config.FastPace);
                }
            }
            else
            {
                if (_currentPiece.Pace != _config.SlowPace)
                {
                    _currentPiece.SetSpeed(_config.SlowPace);
                }
            }
            _currentPiece.transform.position = position;
        }
    }
}