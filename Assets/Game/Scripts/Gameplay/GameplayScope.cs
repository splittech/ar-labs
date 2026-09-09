using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Gameplay
{
    public class GameplayScope : LifetimeScope
    {
        [Header("Create Game Mode")]
        [SerializeField] private CreateGameModeView _createGameModeView;
        [SerializeField] private PudgeSpawnerView _pudgeSpawnerView;

        [Header("Edit Game Mode")]
        [SerializeField] private EditGameModeView _editGameModeView;

        protected override void Configure(IContainerBuilder builder)
        {
            // Bootstrap.
            builder.RegisterEntryPoint<GameplayBootstrap>();

            // Game Mode Switcher.
            builder.Register<GameModeSwitcher>(Lifetime.Singleton);
            builder.Register<GameModeResolver>(Lifetime.Singleton);

            // Empty Game Mode.
            builder.Register<EmptyGameMode>(Lifetime.Singleton);

            // Create Game Mode.
            builder.Register<CreateGameMode>(Lifetime.Singleton);
            builder.RegisterComponent(_createGameModeView);

            // Edit Game Mode.
            builder.Register<EditGameMode>(Lifetime.Singleton);
            builder.RegisterComponent(_editGameModeView);

            // Pudge Spawner.
            builder.Register<PudgeSpawner>(Lifetime.Singleton);
            builder.RegisterComponent(_pudgeSpawnerView);
        }
    }
}
