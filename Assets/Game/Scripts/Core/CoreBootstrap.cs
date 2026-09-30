using Game.Core.AR;
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
        private readonly ARService _ARService;
        private readonly ARServiceLogger _ARServiceLogger;

        public CoreBootstrap(
            InputService inputService,
            InputLogger inputLogger,
            FPSCounter fpsCounter,
            GestureLogger gestureLogger,
            CrossCenterMarkerSpawner crossCenterMarkerSpawner,
            ARService aRService,
            ARServiceLogger aRServiceLogger)
        {
            _inputService = inputService;
            _inputLogger = inputLogger;
            _fpsCounter = fpsCounter;
            _gestureLogger = gestureLogger;
            _crossCenterMarkerSpawner = crossCenterMarkerSpawner;
            _ARService = aRService;
            _ARServiceLogger = aRServiceLogger;
        }

        public void Start()
        {
            _inputService.Enable();
            _ARService.Enable();
            _fpsCounter.Enable();
            _crossCenterMarkerSpawner.Enable();

            _inputLogger.Enable();
            _gestureLogger.Enable();
            _ARServiceLogger.Enable();

            _ARService.SwitchDetectionType(ARService.DetectionType.None);
        }
    }
}
