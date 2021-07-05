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
                EditPhysicsGroup(controller.BeforePlacedPhysics);
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox("Physics Properties For Ragdoll Pieces", MessageType.Info);
                EditPhysicsGroup(controller.AfterPlacedPhysics);
            }
        }

        private void EditPhysicsGroup(PiecePhysics piecePhysics)
        {
            using (var scope = new EditorGUILayout.VerticalScope("Box"))
            {
                EditorWindowHelper.EditFloatValue("AngularDrag", piecePhysics.AngularDrag, piecePhysics.SetAngularDrag);
                EditorWindowHelper.EditFloatValue("LinearDrag", piecePhysics.LinearDrag, piecePhysics.SetLinearDrag);
                EditorWindowHelper.EditFloatValue("Mass", piecePhysics.Mass, piecePhysics.SetMass);
                EditorWindowHelper.EditFloatValue("Gravity", piecePhysics.GravityForce, piecePhysics.SetGravity);
            }
        }
    }
}
