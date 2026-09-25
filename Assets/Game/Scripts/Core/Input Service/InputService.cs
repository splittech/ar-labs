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

            _inputServiceView.OnTapPerformed += OnTapPerformed;

            _inputServiceView.OnPointerPressStarted += OnPointerPressStarted;
            _inputServiceView.OnPointerPressCanceled += OnPointerPressCanceled;

            _inputServiceView.OnDragStarted += OnDragStarted;
            _inputServiceView.OnDragCanceled += OnDragCanceled;
            _inputServiceView.OnDragPerformed += OnDragPerformed;

            _inputServiceView.Enable();
        }

        public void Disable()
        {
            _inputServiceView.OnPointerPositionChanged -= OnPointerPositionChanged;

            _inputServiceView.OnTapPerformed -= OnTapPerformed;

            _inputServiceView.OnPointerPressStarted -= OnPointerPressStarted;
            _inputServiceView.OnPointerPressCanceled -= OnPointerPressCanceled;

            _inputServiceView.OnDragStarted -= OnDragStarted;
            _inputServiceView.OnDragCanceled -= OnDragCanceled;
            _inputServiceView.OnDragPerformed -= OnDragPerformed;

            _inputServiceView.Disable();
        }

        private void OnPointerPositionChanged(Vector2 position)
        {
            _pointerScreenPosition = position;
        }

        private void OnPointerPressStarted()
        {
            InputContext context = new()
            {
                ActionType = ActionType.Press,
                ActionStatus = ActionStatus.Started,
                ScreenPosition = _pointerScreenPosition,
                IsOverUI = _inputUIChecker.CheckPointerOverUI(_pointerScreenPosition)
            };

            _onInputActionPerformed.OnNext(context);
        }

        private void OnPointerPressCanceled()
        {
            InputContext context = new()
            {
                ActionType = ActionType.Press,
                ActionStatus = ActionStatus.Canceled,
                ScreenPosition = _pointerScreenPosition,
                IsOverUI = _inputUIChecker.CheckPointerOverUI(_pointerScreenPosition)
            };

            _onInputActionPerformed.OnNext(context);
        }

        private void OnTapPerformed()
        {
            InputContext context = new()
            {
                ActionType = ActionType.Tap,
                ActionStatus = ActionStatus.Performed,
                ScreenPosition = _pointerScreenPosition,
                IsOverUI = _inputUIChecker.CheckPointerOverUI(_pointerScreenPosition)
            };

            _onInputActionPerformed.OnNext(context);
        }

        private void OnDragStarted()
        {
            InputContext context = new()
            {
                ActionType = ActionType.Drag,
                ActionStatus = ActionStatus.Started,
                ScreenPosition = _pointerScreenPosition,
                IsOverUI = _inputUIChecker.CheckPointerOverUI(_pointerScreenPosition)
            };

            _onInputActionPerformed.OnNext(context);
        }

        private void OnDragCanceled()
        {
            InputContext context = new()
            {
                ActionType = ActionType.Drag,
                ActionStatus = ActionStatus.Canceled,
                ScreenPosition = _pointerScreenPosition,
                IsOverUI = _inputUIChecker.CheckPointerOverUI(_pointerScreenPosition)
            };

            _onInputActionPerformed.OnNext(context);
        }

        private void OnDragPerformed()
        {
            InputContext context = new()
            {
                ActionType = ActionType.Drag,
                ActionStatus = ActionStatus.Performed,
                ScreenPosition = _pointerScreenPosition,
                IsOverUI = _inputUIChecker.CheckPointerOverUI(_pointerScreenPosition)
            };

            _onInputActionPerformed.OnNext(context);
        }
    }
}