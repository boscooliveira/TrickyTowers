using System.Collections;
using GameProject.TrickyTowers.Service;

namespace GameProject.TrickyTowers.Engine
{
    public interface IUnityProxy : IService
    {
        void ExecuteCoroutine(IEnumerator coroutine);
        void EnterGameScene();
        void EnterMenuScene();
    }
}
