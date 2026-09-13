using Game.Core.Input;
using VContainer.Unity;

namespace Game.Core
{
    public class CoreBootstrap : IStartable
    {
        private readonly InputLogger _inputLogger;
        private readonly InputService _inputService;
        private readonly FPSCounter _fpsCounter;

        public CoreBootstrap(InputService inputService, InputLogger inputLogger, FPSCounter fpsCounter)
        {
            _inputService = inputService;
            _inputLogger = inputLogger;
            _fpsCounter = fpsCounter;
        }

        public void Start()
        {
            _fpsCounter.Initialize();
            //_inputLogger.Initialize();

            _inputService.Enable();
        }
    }
}
