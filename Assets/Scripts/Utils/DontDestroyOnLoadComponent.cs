using UnityEngine;

namespace GameProject.TrickyTowers.Utils
{
    public class DontDestroyOnLoadComponent : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}
