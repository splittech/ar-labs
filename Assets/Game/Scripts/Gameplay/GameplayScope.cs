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
        [SerializeField] private PudgeEditorView _pudgeEditorView;
        [SerializeField] private PudgeGestureEditorView _pudgeGestureEditorView;

        [Header("Image Tracking Game Mode")]
        [SerializeField] private ImageTrackingModeView _imageTrackingModeView;
        [SerializeField] private VideoPlayerSpawnerView _videoPlayerSpawnerView;


        [Header("Face Tracking Game Mode")]
        [SerializeField] private FaceTrackingModeView _faceTrackingModeView;

        protected override void Configure(IContainerBuilder builder)
        {
            // Bootstrap.
            builder.RegisterEntryPoint<GameplayBootstrap>();

            // Pudge Merger.
            builder.Register<PudgeMerger>(Lifetime.Singleton);
            builder.RegisterComponent(_pudgeMergerView);

            // Game Mode Switcher.
            builder.Register<GameModeSwitcher>(Lifetime.Singleton);
            builder.Register<GameModeResolver>(Lifetime.Singleton);

            // Empty Game Mode.
            builder.Register<EmptyGameMode>(Lifetime.Singleton);

            // Create Game Mode.
            builder.Register<CreateGameMode>(Lifetime.Singleton);
            builder.RegisterComponent(_createGameModeView);
            builder.Register<SpawnMarkerCreator>(Lifetime.Singleton);
            builder.RegisterComponent(_spawnMarkerCreatorView);
            builder.Register<PudgeSpawner>(Lifetime.Singleton);
            builder.RegisterComponent(_pudgeSpawnerView);

            // Edit Game Mode.
            builder.Register<EditGameMode>(Lifetime.Singleton);
            builder.RegisterComponent(_editGameModeView);
            builder.Register<PudgeEditor>(Lifetime.Singleton);
            builder.RegisterComponent(_pudgeEditorView);
            builder.Register<PudgeGestureEditor>(Lifetime.Singleton);
            builder.RegisterComponent(_pudgeGestureEditorView);

            // Image Tracking Mode.
            builder.Register<ImageTrackingMode>(Lifetime.Singleton);
            builder.RegisterComponent(_imageTrackingModeView);
            builder.Register<VideoPlayerSpawner>(Lifetime.Singleton);
            builder.RegisterComponent(_videoPlayerSpawnerView);

            // Face Tracking Mode.
            builder.Register<FaceTrackingMode>(Lifetime.Singleton);
            builder.RegisterComponent(_faceTrackingModeView);
        }
    }
}
