using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Core.Input
{
    public sealed class InputServiceView : MonoBehaviour
    {
        [SerializeField] private InputActionReference _pointerPositionAction;
        [SerializeField] private InputActionReference _pointerPressAction;
        [SerializeField] private InputActionReference _tapAction;
        [SerializeField] private InputActionReference _dragAction;

        private bool _enabled;

        public event Action<Vector2> OnPointerPositionChanged;

        public event Action OnTapPerformed;

        public event Action OnPointerPressStarted;
        public event Action OnPointerPressCanceled;

        public event Action OnDragStarted;
        public event Action OnDragCanceled;
        public event Action OnDragPerformed;

        public void Enable()
        {
            if (_enabled)
                return;

            _enabled = true;

            _pointerPositionAction.action.performed += HandlePointerPositionPerformed;

            _pointerPressAction.action.started += HandlePointerPressStarted;
            _pointerPressAction.action.canceled += HandlePointerPressCanceled;

            _tapAction.action.performed += HandleTapStartPerformed;
            _dragAction.action.started += HandleDragStarted;
            _dragAction.action.canceled += HandleDragCanceled;
            _dragAction.action.performed += HandleDragPerformed;

            _pointerPositionAction.action.Enable();
            _pointerPressAction.action.Enable();
            _tapAction.action.Enable();
            _dragAction.action.Enable();
        }

        public void Disable()
        {
            if (!_enabled)
                return;

            _enabled = false;

            _pointerPositionAction.action.performed -= HandlePointerPositionPerformed;

            _pointerPressAction.action.started -= HandlePointerPressStarted;
            _pointerPressAction.action.canceled -= HandlePointerPressCanceled;

            _tapAction.action.performed -= HandleTapStartPerformed;
            _dragAction.action.started -= HandleDragStarted;
            _dragAction.action.canceled -= HandleDragCanceled;
            _dragAction.action.performed -= HandleDragPerformed;

            _pointerPositionAction.action.Disable();
            _pointerPressAction.action.Disable();
            _tapAction.action.Disable();
            _dragAction.action.Disable();
        }

        private void HandlePointerPositionPerformed(InputAction.CallbackContext context)
        {
            Vector2 position = context.ReadValue<Vector2>();
            OnPointerPositionChanged?.Invoke(position);
        }

        private void HandleTapStartPerformed(InputAction.CallbackContext _)
        {
            OnTapPerformed?.Invoke();
        }

        private void HandlePointerPressStarted(InputAction.CallbackContext _)
        {
            OnPointerPressStarted?.Invoke();
        }

        private void HandlePointerPressCanceled(InputAction.CallbackContext _)
        {
            OnPointerPressCanceled?.Invoke();
        }

        private void HandleDragStarted(InputAction.CallbackContext _)
        {
            OnDragStarted?.Invoke();
        }

        private void HandleDragCanceled(InputAction.CallbackContext _)
        {
            OnDragCanceled?.Invoke();
        }

        private void HandleDragPerformed(InputAction.CallbackContext _)
        {
            OnDragPerformed?.Invoke();
        }
    }
}