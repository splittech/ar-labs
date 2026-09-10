using R3;

namespace Game.Gameplay
{
    public class CreateGameMode : GameMode
    {
        private readonly CreateGameModeView _createGameModeView;
        private readonly PudgeSpawner _pudgeSpawner;

        private Pudge.State _selectedPudgeState;

        private DisposableBag _disposableBag;

        public CreateGameMode(CreateGameModeView createGameModeView, PudgeSpawner pudgeSpawner)
        {
            _createGameModeView = createGameModeView;
            _pudgeSpawner = pudgeSpawner;
        }

        public override void Enable()
        {
            _pudgeSpawner.Enable();

            _createGameModeView.OnPudgeTypeButtonSelected
                .Subscribe(SwitchPudgeType)
                .AddTo(ref _disposableBag);
        }

        public override void Disable()
        {
            _pudgeSpawner.Disable();
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

            _pudgeSpawner.InititalPudgeState = _selectedPudgeState;
        }
    }
}
