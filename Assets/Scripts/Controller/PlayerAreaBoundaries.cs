using UnityEngine;

namespace GameProject.TrickyTowers.Controller
{
    [System.Serializable]
    public class PlayerAreaBoundaries
    {
        public Transform SpawnerPosition;
        public Transform LimitLeft;
        public Transform LimitRight;
        public Transform LimitBottom;
    }
}