using UnityEngine;

namespace GameProject.TrickyTowers.Config
{
    public interface IGameplayConfig
    {
        int InitialLives { get; }
        float GhostSecsAfterHit { get; }
        float GoalHeight { get; }
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

        public int InitialLives => _initialLives;
        public float GhostSecsAfterHit => _ghostSecsAfterHit;
        public float GoalHeight => _goalHeight;
    }
}
