using System.Collections;
using UnityEngine;
using GameProject.TrickyTowers.Utils;

namespace GameProject.TrickyTowers.Engine
{
    [RequireComponent(typeof(DontDestroyOnLoadComponent))]
    public class UnityProxy : MonoBehaviour, IUnityProxy
    {
        [SerializeField]
        private int _menuSceneIndex;

        [SerializeField]
        private int _gameSceneIndex;

        public void EnterGameScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(_gameSceneIndex);
        }

        public void EnterMenuScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(_menuSceneIndex);
        }

        public void ExecuteCoroutine(IEnumerator coroutine)
        {
            StartCoroutine(coroutine);
        }
    }
}
