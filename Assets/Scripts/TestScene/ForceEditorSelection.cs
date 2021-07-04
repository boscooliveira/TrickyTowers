using UnityEngine;

namespace GameProject.TrickyTowers.TestScene
{
    public class ForceEditorSelection : MonoBehaviour
    {
#if UNITY_EDITOR
        private void Update()
        {
            if (UnityEditor.Selection.activeObject == this)
                return;

            Debug.LogWarning($"Forcing  GameObject Selection: {gameObject.name}");
            UnityEditor.EditorGUIUtility.PingObject(this);
            UnityEditor.Selection.activeObject = this;
        }
#endif
    }
}
