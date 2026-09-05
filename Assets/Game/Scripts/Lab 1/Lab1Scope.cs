using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Lab1
{
    public class Lab1Scope : LifetimeScope
    {
        [SerializeField] private PudgeSpawnerView _pudgeSpawnerView;

        protected override void Configure(IContainerBuilder builder)
        {
            // Bootstrap.
            builder.RegisterEntryPoint<Lab1Bootstrap>();

            // Pudge Spawner.
            builder.Register<PudgeSpawner>(Lifetime.Singleton);
            builder.RegisterInstance(_pudgeSpawnerView);
        }
    }
}
