using R3;

namespace Game.Gameplay
{
    public class EditGameMode : GameMode
    {
        private readonly EditGameModeView _editGameModeView;
        private readonly PudgeEditor _pudgeEditor;

        private DisposableBag _disposableBag;

        public EditGameMode(EditGameModeView editGameModeView, PudgeEditor pudgeEditor)
        {
            _editGameModeView = editGameModeView;
            _pudgeEditor = pudgeEditor;
        }

        public Observable<EditGameModeView.Button> OnButtonPressed => _editGameModeView.OnButtonPressed;

        public override void Enable()
        {
            _pudgeEditor.Enable();

            _editGameModeView.OnButtonPressed
                .Where(button => button == EditGameModeView.Button.AddScale)
                .Subscribe(_ => _pudgeEditor.AddScale())
                .AddTo(ref _disposableBag);

            _editGameModeView.OnButtonPressed
                .Where(button => button == EditGameModeView.Button.SubstractScale)
                .Subscribe(_ => _pudgeEditor.SubstractScale())
                .AddTo(ref _disposableBag);

            _editGameModeView.OnButtonPressed
                .Where(button => button == EditGameModeView.Button.RotateClockwise)
                .Subscribe(_ => _pudgeEditor.RotateClockwise())
                .AddTo(ref _disposableBag);

            _editGameModeView.OnButtonPressed
                .Where(button => button == EditGameModeView.Button.RotateCounterClockwise)
                .Subscribe(_ => _pudgeEditor.RotateCounterClockwise())
                .AddTo(ref _disposableBag);

            _editGameModeView.OnButtonPressed
                .Where(button => button == EditGameModeView.Button.Reset)
                .Subscribe(_ => _pudgeEditor.ResetScaleAndRotation())
                .AddTo(ref _disposableBag);

            _pudgeEditor.SelectedPudge
                .Subscribe(_editGameModeView.UpdateTextPanels)
                .AddTo(ref _disposableBag);

            _pudgeEditor.TotalScaleDelta
                .Subscribe(_editGameModeView.UpdateScaleText)
                .AddTo(ref _disposableBag);

            _pudgeEditor.TotalAngleDelta
                .Subscribe(_editGameModeView.UpdateAngleText)
                .AddTo(ref _disposableBag);
        }

        public override void Disable()
        {
            _pudgeEditor.Disable();

            _editGameModeView.HideAllPanels();

            _disposableBag.Clear();
        }
    }
}
