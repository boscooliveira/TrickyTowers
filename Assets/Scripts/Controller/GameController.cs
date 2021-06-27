using UnityEngine;
using GameProject.TrickyTowers.Service;

namespace GameProject.TrickyTowers.Controller
{
    public class GameController : MonoBehaviour
    {
        [SerializeField]
        private PlayerController _player1;

        [SerializeField]
        private PlayerController _aiPlayer;

        private void Start()
        {
            var configService = ServiceFactory.Instance.Resolve<IGameConfigService>();
            var pieceConfig = configService.PieceConfig;
            var physicsConfig = configService.PhysicsConfig;
            _player1.Initialize(pieceConfig, physicsConfig);
        }
    }
}