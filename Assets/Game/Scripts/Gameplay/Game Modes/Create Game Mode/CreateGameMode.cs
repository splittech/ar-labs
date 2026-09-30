using Game.Core.AR;
using R3;

namespace Game.Gameplay
{
    public class CreateGameMode : GameMode
    {
        private readonly CreateGameModeView _createGameModeView;
        private readonly SpawnMarkerCreator _spawnMarkerCreator;
        private readonly PudgeSpawner _pudgeSpawner;
        private readonly ARService _ARService;

        private Pudge.State _selectedPudgeState;
        private bool _enabled;

        private DisposableBag _disposableBag;

        public CreateGameMode(
            CreateGameModeView createGameModeView,
            PudgeSpawner pudgeSpawner,
            SpawnMarkerCreator spawnMarkerCreator,
            ARService ARService)
        {
            _createGameModeView = createGameModeView;
            _pudgeSpawner = pudgeSpawner;
            _spawnMarkerCreator = spawnMarkerCreator;
            _ARService = ARService;
        }

        public override void Enable()
        {
            if (_enabled)
                return;
            _enabled = true;

            _pudgeSpawner.Enable();
            _spawnMarkerCreator.Enable();

            _ARService.SwitchDetectionType(ARService.DetectionType.Planes);

            _createGameModeView.OnPudgeTypeButtonSelected
                .Subscribe(SwitchPudgeType)
                .AddTo(ref _disposableBag);
        }

        public override void Disable()
        {
            if (!_enabled)
                return;
            _enabled = false;

            _pudgeSpawner.Disable();
            _spawnMarkerCreator.Disable();

            _disposableBag.Clear();
        }

        private void SwitchPudgeType(CreateGameModeView.PudgeTypeButton pudgeTypeButton)
        {
            _selectedPudgeState = pudgeTypeButton switch
            {
                CreateGameModeView.PudgeTypeButton.Normal => Pudge.State.Normal,
                CreateGameModeView.PudgeTypeButton.Happy => Pudge.State.Happy,
                CreateGameModeView.PudgeTypeButton.Sad => Pudge.State.Sad,
                _ => Pudge.State.None,
            };

            _pudgeSpawner.SetInitialPudgeState(_selectedPudgeState);
        }
    }
}
