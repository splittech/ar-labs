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
        [SerializeField] private InputServiceView _inputServiceView;
        [SerializeField] private ARRaycastManager _raycastManager;

        protected override void Configure(IContainerBuilder builder)
        {
            // Bootstrap.
            builder.RegisterEntryPoint<CoreBootstrap>();

            // Input.
            builder.Register<InputService>(Lifetime.Singleton);
            builder.Register<InputUIChecker>(Lifetime.Singleton);
            builder.Register<InputLogger>(Lifetime.Singleton);
            builder.RegisterComponent(_inputServiceView);

            // AR.
            builder.Register<ARRaycastService>(Lifetime.Singleton);
            builder.RegisterComponent(_raycastManager);
        }
    }
}
