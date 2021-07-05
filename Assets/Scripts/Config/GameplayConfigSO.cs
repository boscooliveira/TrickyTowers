using UnityEngine;

namespace GameProject.TrickyTowers.Config
{
    public interface IGameplayConfig
    {
        int InitialLives { get; }
        float GhostSecsAfterHit { get; }
        float GoalHeight { get; }
        float SpawnerHeight { get; }
    }

    [CreateAssetMenu(fileName = "GameplayConfigSO", menuName = "TrickyTowersJoao/GameplayConfig")]
    public class GameplayConfigSO : ScriptableObject, IGameplayConfig
    {
        [SerializeField]
        private int _initialLives;
        [SerializeField]
        private float _ghostSecsAfterHit;
        [SerializeField]
        private float _goalHeight;
        [SerializeField]
        private float _spawnerHeight;

        public int InitialLives => _initialLives;
        public float GhostSecsAfterHit => _ghostSecsAfterHit;
        public float GoalHeight => _goalHeight;
        public float SpawnerHeight => _spawnerHeight;

        public void SetValues(int initialLives, float ghostSecsAfterHit, float goalHeight, float spawnerHeight)
        {
            _initialLives = initialLives;
            _ghostSecsAfterHit = ghostSecsAfterHit;
            _goalHeight = goalHeight;
            _spawnerHeight = spawnerHeight;
        }

        public void SetInitialLives(int value)
        {
            _initialLives = value;
        }

        public void SetGoalHeight(float value)
        {
            _goalHeight = value;
        }

        public void SetSpawnerMinDistance(float value)
        {
            _spawnerHeight = value;
        }

        public void SetGhostSecsAfterHit(float value)
        {
            _ghostSecsAfterHit = value;
        }
    }
}
