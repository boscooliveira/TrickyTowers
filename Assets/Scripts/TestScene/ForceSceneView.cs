using UnityEngine;

namespace GameProject.TrickyTowers.TestScene
{
    public class ForceSceneView : MonoBehaviour
    {
#if UNITY_EDITOR
        private void Update()
        {
            if (UnityEditor.SceneView.lastActiveSceneView == null || 
                UnityEditor.SceneView.lastActiveSceneView.hasFocus)
                return;

            Debug.LogWarning("Forcing SceneView");
            UnityEditor.EditorGUIUtility.PingObject(this);
            UnityEditor.SceneView.lastActiveSceneView.Focus();
        }
#endif
    }
}
