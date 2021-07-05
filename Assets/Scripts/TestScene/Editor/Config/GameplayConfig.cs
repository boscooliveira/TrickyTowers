using GameProject.TrickyTowers.Config;
using UnityEditor;

namespace GameProject.TrickyTowers.TestScene.Editor.Config
{
    public class GameplayConfig : IConfigMenuDrawer
    {
        public EConfigMenuType Option => EConfigMenuType.Gameplay;

        public void Draw(TestSceneController controller)
        {
            using (var scope = new EditorGUILayout.VerticalScope())
            {
                EditorGUILayout.HelpBox("Gameplay Setup", MessageType.Info);
                EditGameplayConfig(controller, controller.GameplayConfigSO);
            }
        }

        private void EditGameplayConfig(TestSceneController controller, GameplayConfigSO gameplayConfig)
        {
            using (var scope = new EditorGUILayout.VerticalScope("Box"))
            {
                bool changed = false;
                changed |= EditorWindowHelper.EditIntValue("Initial Lives", gameplayConfig.InitialLives, gameplayConfig.SetInitialLives);
                if (EditorWindowHelper.EditFloatValue("Goal Height", gameplayConfig.GoalHeight, gameplayConfig.SetGoalHeight) ||
                    EditorWindowHelper.EditFloatValue("Spawner Height", gameplayConfig.SpawnerHeight, gameplayConfig.SetSpawnerMinDistance))
                {
                    changed = true;
                    controller.UpdatePositions(gameplayConfig.GoalHeight, gameplayConfig.SpawnerHeight);
                }
                changed |= EditorWindowHelper.EditFloatValue("Ghost Time After Hit", gameplayConfig.GhostSecsAfterHit, gameplayConfig.SetGhostSecsAfterHit);

                if (changed)
                {
                    EditorUtility.SetDirty(gameplayConfig);
                }
            }
        }
    }
}
