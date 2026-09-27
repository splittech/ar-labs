using System;
using Game.Core;
using Game.Core.AR;
using R3;
using UnityEngine;

namespace Game.Gameplay
{
    public class PudgeGestureEditor
    {
        private readonly PudgeEditor _pudgeEditor;
        private readonly RaycastService _raycastService;
        private readonly GestureService _gestureService;
        private readonly ScreenService _screenService;
        private readonly PudgeGestureEditorView _view;
        private readonly PudgeSpawner _pudgeSpawner;

        private bool _enabled;

        private DisposableBag _disposableBag;

        public PudgeGestureEditor(
            PudgeEditor pudgeEditor,
            RaycastService raycastService,
            GestureService gestureService,
            ScreenService screenService,
            PudgeGestureEditorView view,
            PudgeSpawner pudgeSpawner)
        {
            _pudgeEditor = pudgeEditor;
            _raycastService = raycastService;
            _gestureService = gestureService;
            _screenService = screenService;
            _pudgeSpawner = pudgeSpawner;
            _view = view;
        }

        public void Enable()
        {
            if (_enabled)
                return;
            _enabled = true;

            _gestureService.Enable();

            _gestureService.OnHorizontalSwipe
                .Subscribe(HorizontalSwipeRotatePudge)
                .AddTo(ref _disposableBag);

            _gestureService.OnCross
                .Subscribe(CrossDeletePudge)
                .AddTo(ref _disposableBag);
        }

        public void Disable()
        {
            if (!_enabled)
                return;
            _enabled = false;

            _gestureService.Disable();

            _disposableBag.Clear();
        }

        private void HorizontalSwipeRotatePudge(Swipe swipe)
        {
            Pudge selectedPudge = _pudgeEditor.SelectedPudge.CurrentValue;
            if (selectedPudge == null)
                return;

            float swipePower = Math.Abs(swipe.Vector.x) / _screenService.SceenWidth;
            float swipeSign = Mathf.Sign(swipe.Vector.x);
            float rotationAngle = _view.MaxSwipeRotationAngle * swipePower * swipeSign;

            selectedPudge.RotateBy(rotationAngle, Pudge.EasingType.Damped, swipePower);
        }

        private void CrossDeletePudge(Vector2 crossCenter)
        {
            Debug.Log("Cross delete");

            bool hasCollision = _raycastService.RaycastOnObject(crossCenter, _view.PudgeLayerMask, out var collider);

            if (!hasCollision)
            {
                return;
            }

            if (!collider.TryGetComponent<PudgeView>(out var pudgeView))
                return;

            Pudge pudge = pudgeView.Pudge;

            if (pudge.IsTransforming() || pudge == _pudgeEditor.SelectedPudge.CurrentValue)
                return;

            _pudgeSpawner.DespawnPudge(pudge);
        }
    }
}