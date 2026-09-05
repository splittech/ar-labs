using VContainer.Unity;

namespace Game.Lab1
{
    public class Lab1Bootstrap : IInitializable
    {
        private readonly PudgeSpawner _pudgeSpawner;

        public Lab1Bootstrap(PudgeSpawner pudgeSpawner)
        {
            _pudgeSpawner = pudgeSpawner;
        }

        public void Initialize()
        {
            _pudgeSpawner.Initialize();
        }
    }
}
