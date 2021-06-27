using System;
using System.Collections.Generic;
using GameProject.TrickyTowers.Controller;
using UnityEngine;

namespace GameProject.TrickyTowers.Config
{
    public interface IPieceFactoryConfig
    {
        float DefaultMass { get; }
        List<PieceConfig> Pieces { get; }
    }

    [CreateAssetMenu(fileName = "PieceFactoryConfigSO", menuName = "TrickyTowersJoao/PieceFactoryConfig")]
    public class PieceFactoryConfigSO: ScriptableObject, IPieceFactoryConfig
    {
        [SerializeField]
        private float _defaultMass = 10;

        [SerializeField]
        private List<PieceConfig> _pieces;

        public float DefaultMass => _defaultMass;
        public List<PieceConfig> Pieces => _pieces;
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