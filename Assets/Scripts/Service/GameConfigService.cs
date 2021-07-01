using GameProject.TrickyTowers.Config;
using UnityEngine;

namespace GameProject.TrickyTowers.Service
{
    public interface IGameConfigService
    {
        IPieceFactoryConfig PieceConfig { get; }
        IPhysicsConfig PhysicsConfig { get; }
        IGameplayConfig GameplayConfig { get; }
    }
    
    public class GameConfigService : MonoBehaviour, IGameConfigService
    {
        [SerializeField]
        private PieceFactoryConfigSO _pieceFactoryConfigSO;

        [SerializeField]
        private InGamePhysicsConfigSO _inGamePhysicsConfigSO;

        [SerializeField]
        private GameplayConfigSO _gameplayConfigSO;

        public IPieceFactoryConfig PieceConfig => _pieceFactoryConfigSO;
        public IPhysicsConfig PhysicsConfig => _inGamePhysicsConfigSO;
        public IGameplayConfig GameplayConfig => _gameplayConfigSO;
    }
}
