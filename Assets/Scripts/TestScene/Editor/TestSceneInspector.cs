using UnityEditor;
using UnityEngine.UIElements;

namespace GameProject.TrickyTowers.TestScene.Editor
{
    [CustomEditor(typeof(TestSceneController))]
    public class TestSceneInspector : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement label = new Label("This is a Label in a Custom Editor");
            return label;
        }
    }
}
