using UnityEngine;
using GameProject.TrickyTowers.Service;
using GameProject.TrickyTowers.Engine;

namespace GameProject.TrickyTowers.Controller
{
    public class MenuController : MonoBehaviour
    {
        private void StartGame(bool multiplayer)
        {
            ServiceFactory.Instance.Resolve<IGameplayService>().StartNewGame(multiplayer);
            ServiceFactory.Instance.Resolve<IUnityProxy>().EnterGameScene();
        }

        public void StartSinglePlayerGame()
        {
            StartGame(false);
        }

        public void StartMultiplayerGame()
        {
            StartGame(true);
        }
    }
}