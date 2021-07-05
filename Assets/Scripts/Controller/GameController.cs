using UnityEngine;
using GameProject.TrickyTowers.Service;
using GameProject.TrickyTowers.Engine;
using GameProject.TrickyTowers.Model;

namespace GameProject.TrickyTowers.Controller
{
    public class GameController : MonoBehaviour
    {
        public delegate void OnGameOverDelegate(IPlayerData player, PlayerController controller);
        [SerializeField]
        private PlayerController _playerControllerPrefab;

        [SerializeField]
        private PlayerController _player1;

        [SerializeField]
        private Transform _gameOverScreen;

        [SerializeField]
        private Transform _pauseScreen;

        [SerializeField]
        private Transform _opponentView;

        private IGameData _gameData;

        private void OnGameOver(IPlayerData player, PlayerController controller)
        {
            controller.enabled = false;

            if (player.Lives == 0 && player.Input != EPlayerInputType.Player1)
                return; // ignore CPU lose

            _gameOverScreen?.gameObject.SetActive(true);
        }

        public void Pause()
        {
            _pauseScreen.gameObject.SetActive(true);
            Time.timeScale = 0;
        }

        public void Continue()
        {
            _pauseScreen.gameObject.SetActive(false);
            Time.timeScale = 1;
        }

        public void Menu()
        {
            _pauseScreen.gameObject.SetActive(false);
            ServiceFactory.Instance.Resolve<IUnityProxy>().EnterMenuScene();
        }

        private void Start()
        {
            Time.timeScale = 1;
            var configService = ServiceFactory.Instance.Resolve<IGameConfigService>();
            var gameplayService = ServiceFactory.Instance.Resolve<IGameplayService>();
            var pieceConfig = configService.PieceConfig;
            var physicsConfig = configService.PhysicsConfig;
            _gameData = gameplayService.GetGameData();


            _player1.Initialize(pieceConfig, physicsConfig, gameplayService, _gameData.Player, OnGameOver);
            if (_gameData.Opponent != null)
            {
                _opponentView.gameObject.SetActive(true);
                var playerController = Instantiate(_playerControllerPrefab);
                playerController.transform.position = new Vector3(0, 1000, 0);
                playerController.Initialize(pieceConfig, physicsConfig, gameplayService, _gameData.Opponent, OnGameOver);
                playerController.SetSmallCameraProperties();
            }
        }
    }
}