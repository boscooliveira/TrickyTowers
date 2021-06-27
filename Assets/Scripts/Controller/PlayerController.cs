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

        public void Initialize(IPieceFactoryConfig config, IPhysicsConfig physicsConfig)
        {
            _pieceFactory = new PieceFactory(config, _bounds, physicsConfig);
        }

        private void Start()
        {
            CreateNewPiece();
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
    }
}