using UnityEditor;

namespace GameProject.TrickyTowers.TestScene.Editor
{
    public class TestGameplayWindow : EditorWindow
    {

        static void Init()
        {
            // Get existing open window or if none, make a new one:
            TestGameplayWindow window = (TestGameplayWindow)GetWindow(typeof(TestGameplayWindow));
            window.Show();
        }

        void OnGUI()
        {
            
        }
    }
}
