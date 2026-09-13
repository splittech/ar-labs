using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Gameplay
{
    public class GameplayScope : LifetimeScope
    {
        [Header("Create Game Mode")]
        [SerializeField] private CreateGameModeView _createGameModeView;
        [SerializeField] private SpawnMarkerCreatorView _spawnMarkerCreatorView;
        [SerializeField] private PudgeSpawnerView _pudgeSpawnerView;
        [SerializeField] private PudgeMergerView _pudgeMergerView;

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

            // Spawn Marker Creator.
            builder.Register<SpawnMarkerCreator>(Lifetime.Singleton);
            builder.RegisterComponent(_spawnMarkerCreatorView);

            // Pudge Spawner.
            builder.Register<PudgeSpawner>(Lifetime.Singleton);
            builder.RegisterComponent(_pudgeSpawnerView);

            // Pudge Merger.
            builder.Register<PudgeMerger>(Lifetime.Singleton);
            builder.RegisterComponent(_pudgeMergerView);
        }
    }
}
