using R3;

namespace Game.Gameplay
{
    public class CreateGameMode : GameMode
    {
        private readonly CreateGameModeView _createGameModeView;
        private readonly SpawnMarkerCreator _spawnMarkerCreator;
        private readonly PudgeSpawner _pudgeSpawner;
        private readonly PudgeMerger _pudgeMerger;

        private Pudge.State _selectedPudgeState;
        private bool _enabled;

        private DisposableBag _disposableBag;

        public CreateGameMode(
            CreateGameModeView createGameModeView,
            PudgeSpawner pudgeSpawner,
            SpawnMarkerCreator spawnMarkerCreator,
            PudgeMerger pudgeMerger)
        {
            _createGameModeView = createGameModeView;
            _pudgeSpawner = pudgeSpawner;
            _spawnMarkerCreator = spawnMarkerCreator;
            _pudgeMerger = pudgeMerger;
        }

        public override void Enable()
        {
            if (_enabled)
                return;

            _enabled = true;

            _pudgeSpawner.Enable();
            _spawnMarkerCreator.Enable();

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
