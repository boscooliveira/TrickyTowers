using GameProject.TrickyTowers.Config;
using UnityEditor;

namespace GameProject.TrickyTowers.TestScene.Editor.Config
{
    public class InGamePhysicsConfig : IConfigMenuDrawer
    {
        public EConfigMenuType Option => EConfigMenuType.Physics;

        public void Draw(TestSceneController controller)
        {
            using (var scope = new EditorGUILayout.VerticalScope())
            {
                EditorGUILayout.HelpBox("Physics Properties For Incoming Piece", MessageType.Info);
                EditPhysicsGroup(controller, controller.InGamePhysicsConfigSO.BeforePlacedPhysics);
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("Physics Properties For Ragdoll Pieces", MessageType.Info);
                EditPhysicsGroup(controller, controller.InGamePhysicsConfigSO.AfterPlacedPhysics);
            }
        }

        private void EditPhysicsGroup(TestSceneController controller, PiecePhysics piecePhysics)
        {
            using (var scope = new EditorGUILayout.VerticalScope("Box"))
            {
                bool changed = false;
                changed |= EditorWindowHelper.EditFloatValue("AngularDrag", piecePhysics.AngularDrag, piecePhysics.SetAngularDrag);
                changed |= EditorWindowHelper.EditFloatValue("LinearDrag", piecePhysics.LinearDrag, piecePhysics.SetLinearDrag);
                changed |= EditorWindowHelper.EditFloatValue("Mass", piecePhysics.Mass, piecePhysics.SetMass);
                changed |= EditorWindowHelper.EditFloatValue("Gravity", piecePhysics.GravityForce, piecePhysics.SetGravity);
                if (changed)
                    EditorUtility.SetDirty(controller.InGamePhysicsConfigSO);
            }
        }
    }
}
