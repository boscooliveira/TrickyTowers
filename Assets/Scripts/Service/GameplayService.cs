using GameProject.TrickyTowers.Model;

namespace GameProject.TrickyTowers.Service
{
    public interface IGameplayService
    {
        void StartNewGame(bool multiplayer);
        GameData GetGameData();
    }

    public class GameplayService : IGameplayService
    {
        private GameData _gameData;
        private readonly IGameConfigService _config;

        public GameplayService(IGameConfigService config)
        {
            _config = config;
        }

        public GameData GetGameData()
        {
            return _gameData;
        }

        public void StartNewGame(bool multiplayer)
        {
            _gameData = new GameData(_config.GameplayConfig, multiplayer);
        }
    }
}
