using VContainer.Unity;

namespace Game.Gameplay
{
    public class GameplayBootstrap : IInitializable
    {
        private readonly PudgeSpawner _pudgeSpawner;

        public GameplayBootstrap(PudgeSpawner pudgeSpawner)
        {
            _pudgeSpawner = pudgeSpawner;
        }

        public void Initialize()
        {
            _pudgeSpawner.Initialize();
        }
    }
}
