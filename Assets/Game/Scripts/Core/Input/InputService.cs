using R3;
using UnityEngine;

namespace Game.Core.Input
{
    public class InputService
    {
        private readonly InputServiceView _inputServiceView;
        private readonly InputUIChecker _inputUIChecker;

        private readonly Subject<InputContext> _onInputActionPerformed = new();

        private Vector2 _pointerScreenPosition;

        public Observable<InputContext> OnInputActionPerformed => _onInputActionPerformed;

        public InputService(InputServiceView inputServiceView, InputUIChecker inputUIChecker)
        {
            _inputServiceView = inputServiceView;
            _inputUIChecker = inputUIChecker;
        }

        public void Enable()
        {
            _inputServiceView.OnPointerPositionChanged += OnPointerPositionChanged;

            _inputServiceView.OnTapStarted += OnTapStarted;
            _inputServiceView.OnTapPerformed += OnTapPerformed;

            _inputServiceView.OnDragStarted += OnDragStarted;
            _inputServiceView.OnDragEnded += OnDragEnded;
            _inputServiceView.OnDragHold += OnDragHold;

            _inputServiceView.Enable();
        }

        public void Disable()
        {
            _inputServiceView.OnPointerPositionChanged -= OnPointerPositionChanged;
            _inputServiceView.OnTapPerformed -= OnTapPerformed;
            _inputServiceView.OnDragStarted -= OnDragStarted;
            _inputServiceView.OnDragEnded -= OnDragEnded;
            _inputServiceView.OnDragHold -= OnDragHold;

            _inputServiceView.Disable();
        }

        private void OnPointerPositionChanged(Vector2 position)
        {
            _pointerScreenPosition = position;
        }

        private void OnTapStarted()
        {
            InputContext context = new()
            {
                ActionType = ActionType.TapStarted,
                ScreenPosition = _pointerScreenPosition,
                IsOverUI = _inputUIChecker.CheckPointerOverUI(_pointerScreenPosition)
            };

            _onInputActionPerformed.OnNext(context);
        }

        private void OnTapPerformed()
        {
            InputContext context = new()
            {
                ActionType = ActionType.TapPerformed,
                ScreenPosition = _pointerScreenPosition,
                IsOverUI = _inputUIChecker.CheckPointerOverUI(_pointerScreenPosition)
            };

            _onInputActionPerformed.OnNext(context);
        }

        private void OnDragStarted()
        {
            InputContext context = new()
            {
                ActionType = ActionType.DragStarted,
                ScreenPosition = _pointerScreenPosition,
                IsOverUI = _inputUIChecker.CheckPointerOverUI(_pointerScreenPosition)
            };

            _onInputActionPerformed.OnNext(context);
        }

        private void OnDragEnded()
        {
            InputContext context = new()
            {
                ActionType = ActionType.DragEnded,
                ScreenPosition = _pointerScreenPosition,
                IsOverUI = _inputUIChecker.CheckPointerOverUI(_pointerScreenPosition)
            };

            _onInputActionPerformed.OnNext(context);
        }

        private void OnDragHold()
        {
            InputContext context = new()
            {
                ActionType = ActionType.DragHold,
                ScreenPosition = _pointerScreenPosition,
                IsOverUI = _inputUIChecker.CheckPointerOverUI(_pointerScreenPosition)
            };

            _onInputActionPerformed.OnNext(context);
        }
    }
}