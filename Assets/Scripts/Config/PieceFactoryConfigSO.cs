using System;
using System.Collections.Generic;
using GameProject.TrickyTowers.Controller;
using UnityEngine;

namespace GameProject.TrickyTowers.Config
{
    public interface IPieceFactoryConfig
    {
        float HorizontalMoveDistance { get; }
        float SlowPace { get; }
        float FastPace { get; }
        PhysicsMaterial2D PhysicsMaterial2D { get; }
        List<PieceConfig> Pieces { get; }
    }

    [CreateAssetMenu(fileName = "PieceFactoryConfigSO", menuName = "TrickyTowersJoao/PieceFactoryConfig")]
    public class PieceFactoryConfigSO: ScriptableObject, IPieceFactoryConfig
    {
        [SerializeField]
        private float _horizontalMoveDistance = 0.25f;

        [SerializeField]
        private float _slowPace = 1;

        [SerializeField]
        private float _fastPace = 3;

        [SerializeField]
        private PhysicsMaterial2D _physicsMaterial2D;

        [SerializeField]
        private List<PieceConfig> _pieces;

        public float HorizontalMoveDistance => _horizontalMoveDistance;
        public PhysicsMaterial2D PhysicsMaterial2D => _physicsMaterial2D;
        public List<PieceConfig> Pieces => _pieces;
        public float SlowPace => _slowPace;
        public float FastPace => _fastPace;

        public void CopyValues(IPieceFactoryConfig other)
        {
            _horizontalMoveDistance = other.HorizontalMoveDistance;
            _slowPace = other.SlowPace;
            _fastPace = other.FastPace;
            _physicsMaterial2D = other.PhysicsMaterial2D;
            _pieces = new List<PieceConfig>(other.Pieces);
        }
    }

    [Serializable]
    public class PieceConfig
    {
        [SerializeField]
        private EPieceType _pieceType;

        [SerializeField]
        private PieceController _prefab;

        [SerializeField, Range(1, 100)]
        private float _drawProbability = 1;

        [SerializeField, Range(1, 10)]
        private float _massMultiplyier = 1;

        public PieceController Prefab => _prefab;
        public Sprite PiecePreview => _prefab?.GetComponentInChildren<Sprite>();
        public float DrawProbability => _drawProbability;
        public float MassMultiplyier => _massMultiplyier;
        public EPieceType Type => _pieceType;
    }

    public enum EPieceType
    {
        I,
        L1,
        L2,
        Z1,
        Z2,
        T,
        O
    }
}