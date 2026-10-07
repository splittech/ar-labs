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
        [SerializeField] private ARServiceView _ARServiceView;

        [Header("Light Estimation Service")]
        [SerializeField] private LightEstimationServiceView _lightEstimationServiceView;

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
            builder.Register<ARService>(Lifetime.Singleton);
            builder.RegisterComponent(_ARServiceView);
            builder.Register<RaycastService>(Lifetime.Singleton).AsSelf().As<IRaycastService>();
            builder.RegisterComponent(_ARRaycastManager);
            builder.Register<ARServiceLogger>(Lifetime.Singleton);

            // Light Estimation Service.
            builder.Register<LightEstimationService>(Lifetime.Singleton);
            builder.RegisterComponent(_lightEstimationServiceView);

            // Input Service.
            builder.Register<InputService>(Lifetime.Singleton).AsSelf().As<IInputService>();
            builder.Register<InputUIChecker>(Lifetime.Singleton);
            builder.Register<InputLogger>(Lifetime.Singleton);
            builder.RegisterComponent(_inputServiceView);

            // Tick Service
            builder.RegisterComponent(_tickService).As<ITickService>();

            // FPS Counter.
            builder.Register<FPSCounter>(Lifetime.Singleton);
            builder.RegisterComponent(_fpsCounterView);

            // Gesture Service.
            builder.Register<GestureService>(Lifetime.Singleton).AsSelf().As<IGestureService>();
            builder.RegisterComponent(_gestureServiceView).As<IGestureServiceView>();

            builder.Register<HorizontalSwipeDetector>(Lifetime.Singleton).As<IHorizontalSwipeDetector>();
            builder.RegisterComponent(_horizontalSwipeDetectorView).As<IHorizontalSwipeDetectorView>();

            builder.Register<CrossDetector>(Lifetime.Singleton).As<ICrossDetector>();
            builder.RegisterComponent(_crossDetectorView).As<ICrossDetectorView>();

            builder.Register<CrossCenterMarkerSpawner>(Lifetime.Singleton);
            builder.RegisterComponent(_crossCenterMarkerSpawnerView);

            builder.Register<GestureLogger>(Lifetime.Singleton);

            // Timer Service.
            builder.Register<TimerService>(Lifetime.Singleton).As<ITimerService>();

            // Logging Service.
            builder.Register<LoggingService>(Lifetime.Singleton);
            builder.Register<LoggerFactory>(Lifetime.Singleton);
            builder.RegisterInstance(_loggingServiceConfig);

            // Screen Service.
            builder.Register<ScreenService>(Lifetime.Singleton).AsSelf().As<IScreenService>();
        }
    }
}
