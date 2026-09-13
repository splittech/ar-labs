using VContainer.Unity;

namespace Game.Gameplay
{
    public class GameplayBootstrap : IStartable
    {
        private readonly PudgeSpawner _pudgeSpawner;
        private readonly GameModeSwitcher _gameModeSwitcher;
        private readonly PudgeMerger _pudgeMerger;

        public GameplayBootstrap(
            PudgeSpawner pudgeSpawner,
            GameModeSwitcher gameModeSwitcher,
            PudgeMerger pudgeMerger)
        {
            _pudgeSpawner = pudgeSpawner;
            _gameModeSwitcher = gameModeSwitcher;
            _pudgeMerger = pudgeMerger;
        }

        public void Start()
        {
            _gameModeSwitcher.Initialize();

            _pudgeMerger.Enable();
        }
    }
}
