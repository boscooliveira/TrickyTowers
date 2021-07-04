using UnityEngine;

namespace GameProject.TrickyTowers.Config
{
    public interface IGameplayConfig
    {
        int InitialLives { get; }
        float GhostSecsAfterHit { get; }
        float GoalHeight { get; }
        float SpawnerMinDistance { get; }
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
        private float _spawnerMinDistance;

        public int InitialLives => _initialLives;
        public float GhostSecsAfterHit => _ghostSecsAfterHit;
        public float GoalHeight => _goalHeight;
        public float SpawnerMinDistance => _spawnerMinDistance;

        public void SetValues(int initialLives, float ghostSecsAfterHit, float goalHeight, float spawnerMinDistance)
        {
            _initialLives = initialLives;
            _ghostSecsAfterHit = ghostSecsAfterHit;
            _goalHeight = goalHeight;
            _spawnerMinDistance = spawnerMinDistance;
        }
    }
}
