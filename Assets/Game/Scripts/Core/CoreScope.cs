using Game.Core.AR;
using Game.Core.Input;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using VContainer;
using VContainer.Unity;

namespace Game.Core
{
    public class CoreScope : LifetimeScope
    {
        [Header("Camera")]
        [SerializeField] private Camera _camera;

        [Header("AR")]
        [SerializeField] private ARRaycastManager _ARRaycastManager;

        [Header("Input Service")]
        [SerializeField] private InputServiceView _inputServiceView;

        [Header("Tick Service")]
        [SerializeField] private TickService _tickService;

        [Header("FPS Counter")]
        [SerializeField] private FPSCounterView _fpsCounterView;

        [Header("Gesture Service")]
        [SerializeField] private GestureServiceView _gestureServiceView;
        [SerializeField] private HorizontalSwipeDetectorView _horizontalSwipeDetectorView;
        [SerializeField] private CrossDetectorView _crossDetectorView;
        [SerializeField] private CrossCenterMarkerSpawnerView _crossCenterMarkerSpawnerView;

        [Header("Logging Service")]
        [SerializeField] private LoggingServiceConfig _loggingServiceConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            // Bootstrap.
            builder.RegisterEntryPoint<CoreBootstrap>();

            // Camera.
            builder.RegisterComponent(_camera);

            // AR.
            builder.Register<RaycastService>(Lifetime.Singleton);
            builder.RegisterComponent(_ARRaycastManager);

            // Input Service.
            builder.Register<InputService>(Lifetime.Singleton);
            builder.Register<InputUIChecker>(Lifetime.Singleton);
            builder.Register<InputLogger>(Lifetime.Singleton);
            builder.RegisterComponent(_inputServiceView);

            // Tick Service
            builder.RegisterComponent(_tickService);

            // FPS Counter.
            builder.Register<FPSCounter>(Lifetime.Singleton);
            builder.RegisterComponent(_fpsCounterView);

            // Gesture Service.
            builder.Register<GestureService>(Lifetime.Singleton);
            builder.RegisterComponent(_gestureServiceView);

            builder.Register<HorizontalSwipeDetector>(Lifetime.Singleton);
            builder.RegisterComponent(_horizontalSwipeDetectorView);

            builder.Register<CrossDetector>(Lifetime.Singleton);
            builder.RegisterComponent(_crossDetectorView);

            builder.Register<CrossCenterMarkerSpawner>(Lifetime.Singleton);
            builder.RegisterComponent(_crossCenterMarkerSpawnerView);

            builder.Register<GestureLogger>(Lifetime.Singleton);

            // Timer Service.
            builder.Register<TimerService>(Lifetime.Singleton);

            // Logging Service.
            builder.Register<LoggingService>(Lifetime.Singleton);
            builder.Register<LoggerFactory>(Lifetime.Singleton);
            builder.RegisterInstance(_loggingServiceConfig);

            // Screen Service.
            builder.Register<ScreenService>(Lifetime.Singleton);
        }
    }
}
