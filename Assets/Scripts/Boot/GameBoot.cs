using GameProject.TrickyTowers.Service;
using GameProject.TrickyTowers.Engine;
using UnityEngine;

namespace GameProject.TrickyTowers.Boot
{
    public class GameBoot : MonoBehaviour
    {
        [SerializeField]
        private UnityProxy _proxy;

        [SerializeField]
        private GameConfigService _configService;

        [SerializeField]
        private int _initialSceneIndex;

        private void Awake()
        {
            InjectServices();
            UnityEngine.SceneManagement.SceneManager.LoadScene(_initialSceneIndex);
        }

        private void InjectServices()
        {
            ServiceFactory.Instance.Register<IUnityProxy>(_proxy);
            ServiceFactory.Instance.Register<IGameConfigService>(_configService);
        }
    }
}
