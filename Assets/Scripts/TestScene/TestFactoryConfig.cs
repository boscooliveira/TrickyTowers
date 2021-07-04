using UnityEngine;
using GameProject.TrickyTowers.Config;
using System.Collections.Generic;

namespace GameProject.TrickyTowers.TestScene
{
    public class TestFactoryConfig : IPieceFactoryConfig
    {
        public float HorizontalMoveDistance { get; set; }
        public PhysicsMaterial2D PhysicsMaterial2D { get; set; }
        public List<PieceConfig> Pieces { get; set; }
        public float SlowPace { get; set; }
        public float FastPace { get; set; }

        public TestFactoryConfig(PieceFactoryConfigSO pieceFactoryConfigSO)
        {
            HorizontalMoveDistance = pieceFactoryConfigSO.HorizontalMoveDistance;
            SlowPace = pieceFactoryConfigSO.SlowPace;
            FastPace = pieceFactoryConfigSO.FastPace;
            PhysicsMaterial2D = pieceFactoryConfigSO.PhysicsMaterial2D;
            Pieces = pieceFactoryConfigSO.Pieces;
        }

        public void UpdateScriptableobject(PieceFactoryConfigSO pieceFactoryConfigSO)
        {
            pieceFactoryConfigSO.CopyValues(pieceFactoryConfigSO);
        }
    }
}
