using Game.Core.AR;
using Game.Core.Input;
using R3;

namespace Game.Gameplay
{
    public class PudgeEditor
    {
        private readonly InputService _inputService;
        private readonly RaycastService _raycastService;
        private readonly PudgeEditorView _pudgeEditorView;

        private bool _enabled;

        private DisposableBag _disposableBag;

        private ReactiveProperty<Pudge> _selectedPudge = new();
        private ReactiveProperty<float> _scaleDelta = new();
        private ReactiveProperty<float> _angleDelta = new();

        public ReadOnlyReactiveProperty<Pudge> SelectedPudge => _selectedPudge;
        public ReadOnlyReactiveProperty<float> ScaleDelta => _scaleDelta;
        public ReadOnlyReactiveProperty<float> AngleDelta => _angleDelta;

        public PudgeEditor(InputService inputService, RaycastService raycastService, PudgeEditorView pudgeEditorView)
        {
            _inputService = inputService;
            _raycastService = raycastService;
            _pudgeEditorView = pudgeEditorView;
        }

        public void Enable()
        {
            if (_enabled)
                return;

            _enabled = true;

            SelectPudge(null);

            _scaleDelta.Value = 0f;
            _angleDelta.Value = 0f;

            _inputService.OnInputActionPerformed
                .Where(context =>
                    context.ActionType == ActionType.Tap &&
                    context.ActionStatus == ActionStatus.Performed &&
                    !context.IsOverUI)
                .Subscribe(TryGetPudge)
                .AddTo(ref _disposableBag);
        }

        public void Disable()
        {
            if (!_enabled)
                return;

            _enabled = false;

            SelectPudge(null);

            _disposableBag.Clear();
        }

        public void AddScale()
        {
            Pudge selectedPudge = _selectedPudge.CurrentValue;

            if (selectedPudge == null)
                return;

            _angleDelta.Value += _pudgeEditorView.ScaleDelta;

            selectedPudge.SetScale(selectedPudge.CurrentScale + _pudgeEditorView.ScaleDelta);
        }

        public void SubstractScale()
        {
            Pudge selectedPudge = _selectedPudge.CurrentValue;

            if (selectedPudge == null)
                return;

            _angleDelta.Value -= _pudgeEditorView.ScaleDelta;

            selectedPudge.SetScale(selectedPudge.CurrentScale - _pudgeEditorView.ScaleDelta);
        }

        private void TryGetPudge(InputContext context)
        {
            bool hasCollision = _raycastService.RaycastOnObject(
                context.ScreenPosition,
                _pudgeEditorView.PudgeInteractableLayer,
                out var collider);

            if (!hasCollision)
                return;

            if (!collider.TryGetComponent<PudgeView>(out var pudgeView))
                return;

            SelectPudge(pudgeView.Pudge);
        }

        private void SelectPudge(Pudge pudge)
        {
            _scaleDelta.Value = 0f;
            _angleDelta.Value = 0f;

            _selectedPudge.CurrentValue?.Deselect();
            _selectedPudge.Value = pudge;
            pudge?.Select();
        }
    }
}