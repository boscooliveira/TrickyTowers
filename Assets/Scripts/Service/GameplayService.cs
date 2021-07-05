using GameProject.TrickyTowers.Model;

namespace GameProject.TrickyTowers.Service
{
    public interface IGameplayService : IService
    {
        void StartNewGame(bool multiplayer);
        IGameData GetGameData();
    }

    public class GameplayService : IGameplayService
    {
        private IGameData _gameData;
        private readonly IGameConfigService _config;

        public GameplayService(IGameConfigService config)
        {
            _config = config;
        }

        public IGameData GetGameData()
        {
            return _gameData;
        }

        public void StartNewGame(bool multiplayer)
        {
            _gameData = new GameData(_config.GameplayConfig, multiplayer);
        }
    }
}
