using GameProject.TrickyTowers.Config;

namespace GameProject.TrickyTowers.Model
{
    public class GameData
    {
        public event System.Action<GameData> OnGameFinished;

        public PlayerData Player { get; private set; }
        public PlayerData Opponent { get; private set; }
        public IGameplayConfig Config { get; private set; }

        public bool PlayerWon { get; private set; }
        public bool Finished { get; private set; }

        public void SetWinner(bool localPlayerWon)
        {
            PlayerWon = localPlayerWon;
            Finished = true;
            OnGameFinished?.Invoke(this);
        }

        public GameData(IGameplayConfig config, bool addAIOpponent)
        {
            Config = config;
            Player = new PlayerData(config.InitialLives, EPlayerInputType.Player1);
            
            if (addAIOpponent)
            {
                Opponent = new PlayerData(config.InitialLives, EPlayerInputType.RandomTargetAI);
            }
        }
    }
}
