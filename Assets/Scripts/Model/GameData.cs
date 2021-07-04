using GameProject.TrickyTowers.Config;

namespace GameProject.TrickyTowers.Model
{
    public interface IGameData
    {
        event System.Action<GameData> OnGameFinished;

        IPlayerData Player { get; }
        IPlayerData Opponent { get; }
        IGameplayConfig Config { get; }

        bool PlayerWon { get; }
        bool Finished { get; }

    }

    public class GameData : IGameData
    {
        public event System.Action<GameData> OnGameFinished;

        public IPlayerData Player { get; private set; }
        public IPlayerData Opponent { get; private set; }
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
