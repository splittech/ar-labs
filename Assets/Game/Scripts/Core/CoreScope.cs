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
        [Header("AR")]
        [SerializeField] private ARRaycastManager _raycastManager;

        [Header("Input Service")]
        [SerializeField] private InputServiceView _inputServiceView;

        [Header("FPS Counter")]
        [SerializeField] private FPSCounterConfig _fpsCounterConfig;
        [SerializeField] private FPSCounterView _fpsCounterView;

        protected override void Configure(IContainerBuilder builder)
        {
            // Bootstrap.
            builder.RegisterEntryPoint<CoreBootstrap>();

            // AR.
            builder.Register<ARRaycastService>(Lifetime.Singleton);
            builder.RegisterComponent(_raycastManager);

            // Input.
            builder.Register<InputService>(Lifetime.Singleton);
            builder.Register<InputUIChecker>(Lifetime.Singleton);
            builder.Register<InputLogger>(Lifetime.Singleton);
            builder.RegisterComponent(_inputServiceView);

            // FPS Counter.
            builder.Register<FPSCounter>(Lifetime.Singleton);
            builder.RegisterInstance(_fpsCounterConfig);
            builder.RegisterComponent(_fpsCounterView);
        }
    }
}
