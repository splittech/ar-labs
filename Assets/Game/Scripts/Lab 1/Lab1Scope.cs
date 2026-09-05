using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Game.Lab1
{
    public class Lab1Scope : LifetimeScope
    {
        [SerializeField] private PudgeSpawnerView _pudgeSpawnerView;
        [SerializeField] private Button _despawnAllPudgesButton;

        protected override void Configure(IContainerBuilder builder)
        {
            // Bootstrap.
            builder.RegisterEntryPoint<Lab1Bootstrap>();

            // Pudge Spawner.
            builder.Register<PudgeSpawner>(Lifetime.Singleton);
            builder.RegisterComponent(_pudgeSpawnerView);
            builder.RegisterComponent(_despawnAllPudgesButton);
        }
    }
}
