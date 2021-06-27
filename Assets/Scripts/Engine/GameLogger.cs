using UnityEngine;

namespace GameProject.TrickyTowers.Engine
{
    public static class GameLogger
    {
        // Start is called before the first frame update
        public static void LogError(string error)
        {
            Debug.LogError(error);
        }
    }
}
