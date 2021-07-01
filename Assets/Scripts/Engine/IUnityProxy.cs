using System.Collections;

namespace GameProject.TrickyTowers.Engine
{
    public interface IUnityProxy
    {
        void ExecuteCoroutine(IEnumerator coroutine);
        void EnterGameScene();
        void EnterMenuScene();
    }
}
