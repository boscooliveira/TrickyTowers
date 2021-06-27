using GameProject.TrickyTowers.Config;
using UnityEngine;

namespace GameProject.TrickyTowers.Service
{
    public interface IGameConfigService
    {
        IPieceFactoryConfig PieceConfig { get; }
        IPhysicsConfig PhysicsConfig { get; }
    }
    
    public class GameConfigService : MonoBehaviour, IGameConfigService
    {
        [SerializeField]
        private PieceFactoryConfigSO _pieceFactoryConfigSO;

        [SerializeField]
        private InGamePhysicsConfigSO _inGamePhysicsConfigSO;

        public IPieceFactoryConfig PieceConfig => _pieceFactoryConfigSO;
        public IPhysicsConfig PhysicsConfig => _inGamePhysicsConfigSO;
    }
}
