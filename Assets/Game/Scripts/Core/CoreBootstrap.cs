using Game.Core.Input;
using VContainer.Unity;

namespace Game.Core
{
    public class CoreBootstrap : IStartable
    {
        private readonly InputLogger _inputLogger;
        private readonly InputService _inputService;
        private readonly FPSCounter _fpsCounter;
        private readonly GestureLogger _gestureLogger;
        private readonly CrossCenterMarkerSpawner _crossCenterMarkerSpawner;

        public CoreBootstrap(
            InputService inputService,
            InputLogger inputLogger,
            FPSCounter fpsCounter,
            GestureLogger gestureLogger,
            CrossCenterMarkerSpawner crossCenterMarkerSpawner)
        {
            _inputService = inputService;
            _inputLogger = inputLogger;
            _fpsCounter = fpsCounter;
            _gestureLogger = gestureLogger;
            _crossCenterMarkerSpawner = crossCenterMarkerSpawner;
        }

        public void Start()
        {
            _fpsCounter.Enable();
            _inputService.Enable();

            _crossCenterMarkerSpawner.Enable();

            _inputLogger.Enable();
            _gestureLogger.Enable();
        }
    }
}
