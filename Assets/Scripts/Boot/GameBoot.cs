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

        private void Awake()
        {
            InjectServices();
            _proxy.EnterMenuScene();
        }

        private void InjectServices()
        {
            ServiceFactory.Instance.Register<IUnityProxy>(_proxy);
            ServiceFactory.Instance.Register<IGameConfigService>(_configService);
            ServiceFactory.Instance.Register<IGameplayService>(new GameplayService(_configService));
        }
    }
}
