using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Gameplay
{
    public class GameplayScope : LifetimeScope
    {
        [SerializeField] private PudgeSpawnerView _pudgeSpawnerView;

        protected override void Configure(IContainerBuilder builder)
        {
            // Bootstrap.
            builder.RegisterEntryPoint<GameplayBootstrap>();

            // Pudge Spawner.
            builder.Register<PudgeSpawner>(Lifetime.Singleton);
            builder.RegisterComponent(_pudgeSpawnerView);
        }
    }
}
