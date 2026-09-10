using VContainer.Unity;

namespace Game.Gameplay
{
    public class GameplayBootstrap : IStartable
    {
        private readonly PudgeSpawner _pudgeSpawner;
        private readonly GameModeSwitcher _gameModeSwitcher;

        public GameplayBootstrap(
            PudgeSpawner pudgeSpawner,
            GameModeSwitcher gameModeSwitcher)
        {
            _pudgeSpawner = pudgeSpawner;
            _gameModeSwitcher = gameModeSwitcher;
        }

        public void Start()
        {
            _gameModeSwitcher.Initialize();
        }
    }
}
