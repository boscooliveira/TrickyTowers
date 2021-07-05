using GameProject.TrickyTowers.Utils;
using GameProject.TrickyTowers.Config;
using GameProject.TrickyTowers.Controller;
using System.Collections.Generic;
using UnityEngine;

namespace GameProject.TrickyTowers.Model
{
    public class PieceFactory
    {
        private readonly IPieceFactoryConfig _pieceConfig;
        private readonly IPhysicsConfig _physicsConfig;
        private readonly Dictionary<EPieceType, ObjectPool<PieceController>> _pools;
        private readonly float _sumProbabilities;
        private PlayerAreaBoundaries _bounds;

        public PieceFactory(IPieceFactoryConfig config, PlayerAreaBoundaries bounds, IPhysicsConfig physicsConfig)
        {
            _pieceConfig = config;
            _bounds = bounds;
            _physicsConfig = physicsConfig;

            _pools = new Dictionary<EPieceType, ObjectPool<PieceController>>(_pieceConfig.Pieces.Count);

            var pieces = config.Pieces;
            _sumProbabilities = 0;
            foreach (PieceConfig piece in pieces)
            {
                _sumProbabilities += piece.DrawProbability;
                _pools[piece.Type] = new ObjectPool<PieceController>(() => CreateNewPiece(piece, _bounds));
            }
        }

        private PieceController CreateNewPiece(PieceConfig config, PlayerAreaBoundaries bounds)
        {
            var obj = Object.Instantiate(config.Prefab, bounds.SpawnerPosition.position, Quaternion.identity, bounds.SpawnerPosition);
            obj.SetConfig(_pieceConfig, _physicsConfig, bounds);
            return obj;
        }

        public PieceController GetPiece()
        {
            if (Mathf.Approximately(_sumProbabilities, 0) || _sumProbabilities < 0)
            {
                return _pools[_pieceConfig.Pieces[0].Type].GetObject();
            }

            var randNumber = Random.Range(0f, 1f) * _sumProbabilities;
            var pieces = _pieceConfig.Pieces;
            foreach (PieceConfig piece in pieces)
            {
                randNumber -= piece.DrawProbability;
                if (randNumber <= 0)
                {
                    return _pools[piece.Type].GetObject();
                }
            }

            return _pools[_pieceConfig.Pieces[_pieceConfig.Pieces.Count - 1].Type].GetObject();
        }
    }
}
