using UnityEditor;
using UnityEngine;

namespace GameProject.TrickyTowers.TestScene.Editor
{
    [CustomEditor(typeof(TestSceneController))]
    public class TestSceneInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Open Config Window"))
            {
                OpenConfigWindow(target as TestSceneController);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void OpenConfigWindow(TestSceneController testSceneController)
        {
            GameConfigWindow.Init();
        }
    }
}
