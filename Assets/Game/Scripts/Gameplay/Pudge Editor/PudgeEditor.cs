using Game.Core.AR;
using Game.Core.Input;
using R3;
using UnityEngine;

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
        private ReactiveProperty<float> _totalScaleDelta = new();
        private ReactiveProperty<float> _totalRotationDelta = new();

        public ReadOnlyReactiveProperty<Pudge> SelectedPudge => _selectedPudge;
        public ReadOnlyReactiveProperty<float> TotalScaleDelta => _totalScaleDelta;
        public ReadOnlyReactiveProperty<float> TotalAngleDelta => _totalRotationDelta;

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

            _totalScaleDelta.Value = 0f;
            _totalRotationDelta.Value = 0f;

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
            ChangeScale(_pudgeEditorView.ScaleDelta);
        }

        public void SubstractScale()
        {
            ChangeScale(-_pudgeEditorView.ScaleDelta);
        }

        public void RotateClockwise()
        {
            ChangeRotation(_pudgeEditorView.RotationDelta);
        }

        public void RotateCounterClockwise()
        {
            ChangeRotation(-_pudgeEditorView.RotationDelta);
        }

        public void ResetScaleAndRotation()
        {
            ChangeScale(-_totalScaleDelta.CurrentValue);
            ChangeRotation(-_totalRotationDelta.CurrentValue);
        }

        private void ChangeScale(float scaleDelta)
        {
            Pudge selectedPudge = _selectedPudge.CurrentValue;

            if (selectedPudge == null)
                return;

            if (selectedPudge.CurrentScale + scaleDelta < Mathf.Epsilon)
                return;

            selectedPudge.ScaleTo(selectedPudge.CurrentScale + scaleDelta, Pudge.EasingType.Instant);

            _totalScaleDelta.Value += scaleDelta;
        }

        private void ChangeRotation(float angleDelta)
        {
            Pudge selectedPudge = _selectedPudge.CurrentValue;

            if (selectedPudge == null)
                return;

            Quaternion pudgeRotation = selectedPudge.CurrentPose.rotation;
            Quaternion deltaRotation = Quaternion.AngleAxis(angleDelta, Vector3.up);

            selectedPudge.RotateTo(pudgeRotation * deltaRotation, Pudge.EasingType.Instant);

            _totalRotationDelta.Value += angleDelta;
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

            Pudge pudge = pudgeView.Pudge;

            if (pudge == _selectedPudge.CurrentValue || pudge.IsTransforming())
                return;

            SelectPudge(pudge);
        }

        private void SelectPudge(Pudge pudge)
        {
            _totalScaleDelta.Value = 0f;
            _totalRotationDelta.Value = 0f;

            _selectedPudge.CurrentValue?.Deselect();
            _selectedPudge.Value = pudge;
            pudge?.Select();
        }
    }
}