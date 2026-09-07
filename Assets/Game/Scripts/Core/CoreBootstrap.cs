using Game.Core.Input;
using VContainer.Unity;

namespace Game.Core
{
    public class CoreBootstrap : IInitializable
    {
        private readonly InputLogger _inputLogger;
        private InputService _inputService;

        public CoreBootstrap(InputService inputService, InputLogger inputLogger)
        {
            _inputService = inputService;
            _inputLogger = inputLogger;
        }

        public void Initialize()
        {
            _inputLogger.Initialize();

            _inputService.Enable();
        }
    }
}
