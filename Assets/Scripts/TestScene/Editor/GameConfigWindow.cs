using UnityEditor;
using UnityEngine;

namespace GameProject.TrickyTowers.TestScene.Editor
{
    public class GameConfigWindow : EditorWindow
    {
        private static TestSceneController _testScene;
        private ConfigWindowMenu _configMenu;
        private bool _testSceneNotFound;

        [MenuItem("TrickyTowers/TestScene/Config", priority = 11)]
        public static void Init()
        {
            // Get existing open window or if none, make a new one:
            var window = (GameConfigWindow) GetWindow(
                typeof(GameConfigWindow), 
                false, 
                "Game Setup", true);

            _testScene = FindObjectOfType<TestSceneController>();
            if (_testScene == null)
            {
                EditorUtility.DisplayDialog("Warning", "Consider re-openning this window after entering the test scene", "ok");
            }
            window.Show();
        }

        private void OnDestroy()
        {
            _testScene = null;
        }

        private void Awake()
        {
            Refresh();
        }

        private void Refresh()
        {
            _testScene = FindObjectOfType<TestSceneController>();
            _testSceneNotFound = _testScene == null;
            if (_testSceneNotFound)
                return;

            _configMenu = new ConfigWindowMenu();
            _testSceneNotFound = _testScene == null;
        }

        public void OnGUI()
        {
            if (!_testSceneNotFound && (_testScene == null || _configMenu == null))
            {
                Refresh();
            }
            if (_testScene == null || EditorApplication.isPlaying)
            {
                GUI.enabled = false;
            }
            if (_testSceneNotFound)
            {
                if (GUILayout.Button("Refresh Window"))
                {
                    Refresh();
                }
            }
            _configMenu?.Draw(_testScene);
            GUI.enabled = true;
        }
    }
}
