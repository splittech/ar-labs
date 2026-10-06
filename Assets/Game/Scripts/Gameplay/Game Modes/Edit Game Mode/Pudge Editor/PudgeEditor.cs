using Game.Core.AR;
using Game.Core.Input;
using R3;
using UnityEngine;

namespace Game.Gameplay
{
    public class PudgeEditor : IPudgeEditor
    {
        private readonly InputService _inputService;
        private readonly RaycastService _raycastService;
        private readonly PudgeEditorView _pudgeEditorView;

        private bool _enabled;

        private DisposableBag _disposableBag;

        private ReactiveProperty<Pudge> _selectedPudge = new();
        private ReactiveProperty<Pudge> _previousSelectedPudge = new();
        private ReactiveProperty<float> _totalScaleDelta = new();
        private ReactiveProperty<float> _totalRotationDelta = new();

        public ReadOnlyReactiveProperty<Pudge> SelectedPudge => _selectedPudge;
        public ReadOnlyReactiveProperty<Pudge> PreviousSelectedPudge => _previousSelectedPudge;
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

            _totalScaleDelta.Value = 0f;
            _totalRotationDelta.Value = 0f;

            _inputService.OnInputActionPerformed
                .Where(context =>
                    context.ActionType == ActionType.Tap &&
                    context.ActionStatus == ActionStatus.Performed &&
                    !context.IsOverUI)
                .Subscribe(TrySelectPudge)
                .AddTo(ref _disposableBag);
        }

        public void Disable()
        {
            if (!_enabled)
                return;

            _enabled = false;

            DeselectCurrentPudge();
            _selectedPudge.Value = null;

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

        private void DeselectCurrentPudge()
        {
            _previousSelectedPudge.Value = _selectedPudge.Value;
            if (_selectedPudge.Value != null && _selectedPudge.Value.IsValid)
                _selectedPudge.Value.Deselect();
        }

        private void TrySelectPudge(InputContext context)
        {
            DeselectCurrentPudge();

            if (!TryRaycastOnPudge(context.ScreenPosition, out Pudge pudge))
            {
                _selectedPudge.Value = null;
                return;
            }

            if (!FilterPudge(pudge))
            {
                _selectedPudge.Value = null;
                return;
            }

            _totalScaleDelta.Value = 0f;
            _totalRotationDelta.Value = 0f;

            _selectedPudge.Value = pudge;
            pudge.Select();
        }

        private bool TryRaycastOnPudge(Vector2 screenPosition, out Pudge pudge)
        {
            pudge = null;

            bool hasCollision = _raycastService.RaycastOnObject(
                screenPosition,
                _pudgeEditorView.PudgeInteractableLayer,
                out var collider);

            if (!hasCollision)
                return false;

            if (!collider.TryGetComponent<IPudgeView>(out var pudgeView))
                return false;

            pudge = pudgeView.Pudge;
            return true;
        }

        private bool FilterPudge(Pudge pudge)
        {
            return
                pudge != _selectedPudge.CurrentValue ||
                !pudge.IsTransforming();
        }
    }
}