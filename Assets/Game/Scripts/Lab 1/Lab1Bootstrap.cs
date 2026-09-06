using Game.Core.Input;
using VContainer.Unity;

namespace Game.Lab1
{
    public class Lab1Bootstrap : IInitializable
    {
        private readonly PudgeSpawner _pudgeSpawner;
        private readonly InputService _inputService;

        public Lab1Bootstrap(PudgeSpawner pudgeSpawner, InputService inputService)
        {
            _pudgeSpawner = pudgeSpawner;
            _inputService = inputService;
        }

        public void Initialize()
        {
            _inputService.Enable();
            _pudgeSpawner.Initialize();
        }
    }
}
