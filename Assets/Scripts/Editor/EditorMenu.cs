using UnityEditor;
using UnityEditor.SceneManagement;

namespace GameProject.TrickyTowers.Editor
{
    [InitializeOnLoadAttribute]
    public class EditorMenu
    {
        [MenuItem("TrickyTowers/PlayGame _%J")]
        static void PlayGame()
        {
            if (EditorApplication.isPlaying)
            {
                return;
            }
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                return;
            }

            var pathOfFirstScene = EditorBuildSettings.scenes[0].path;
            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(pathOfFirstScene);
            EditorSceneManager.playModeStartScene = sceneAsset;
            EditorApplication.isPlaying = true;
        }

        [MenuItem("TrickyTowers/TestScene _%T")]
        static void EnterTestScene()
        {
            if (EditorApplication.isPlaying || EditorSceneManager.GetActiveScene().name == "GameplayTest")
            {
                return;
            }
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                return;
            }

            EditorSceneManager.OpenScene("Assets/Scenes/GameplayTest.unity");
        }

        static EditorMenu()
        {
            EditorApplication.playModeStateChanged += LoadDefaultScene;
        }

        static void LoadDefaultScene(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            }

            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                EditorSceneManager.playModeStartScene = null;
            }
        }
    }
}