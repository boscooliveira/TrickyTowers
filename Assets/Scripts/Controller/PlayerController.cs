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

        public void Initialize(IPieceFactoryConfig config, IPhysicsConfig physicsConfig)
        {
            _config = config;
            _pieceFactory = new PieceFactory(config, _bounds, physicsConfig);
        }

        private void Start()
        {
            CreateNewPiece();
        }

        public void RotatePiece(float x)
        {
            if (_currentPiece == null)
                return;

            _currentPiece.Rotate();
        }

        private void CreateNewPiece()
        {
            var piece = _pieceFactory.CreatePiece();
            piece.OnMoveFinished += OnPieceMoveFinished;
            piece.OnDisabled += OnPieceMoveFinished;
            _currentPiece = piece;
        }

        private void OnPieceMoveFinished(IPoolableItem controller)
        {
            if (_currentPiece == (object)controller)
                CreateNewPiece();
        }

        public void MovePiece(float direction)
        {
            if (_currentPiece == null)
                return;

            var position = _currentPiece.transform.position;
            position.x += direction * _config.HorizontalMoveDistance;
            _currentPiece.transform.position = position;
        }
    }
}