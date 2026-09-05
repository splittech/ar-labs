using UnityEngine;
using UnityEngine.XR.ARFoundation;
using VContainer;
using VContainer.Unity;

namespace Game.Core
{
    public class CoreScope : LifetimeScope
    {
        [SerializeField] private InputService _inputService;
        [SerializeField] private ARRaycastManager _raycastManager;

        protected override void Configure(IContainerBuilder builder)
        {
            // Bootstrap.
            builder.RegisterEntryPoint<CoreBootstrap>();

            // Input.
            builder.RegisterComponent(_inputService);

            // AR.
            builder.RegisterComponent(_raycastManager);
        }
    }
}
