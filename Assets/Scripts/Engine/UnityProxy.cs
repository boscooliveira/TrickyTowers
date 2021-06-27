using System.Collections;
using UnityEngine;
using GameProject.TrickyTowers.Utils;

namespace GameProject.TrickyTowers.Engine
{
    [RequireComponent(typeof(DontDestroyOnLoadComponent))]
    public class UnityProxy : MonoBehaviour, IUnityProxy
    {
        public void ExecuteCoroutine(IEnumerator coroutine)
        {
            StartCoroutine(coroutine);
        }
    }
}
