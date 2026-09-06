using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Core.Input
{
    public sealed class InputServiceView : MonoBehaviour
    {
        [SerializeField] private InputActionReference _pointerPositionAction;
        [SerializeField] private InputActionReference _tapStartAction;
        [SerializeField] private InputActionReference _dragCurrentAction;

        private bool _enabled;

        public event Action<Vector2> OnPointerPositionChanged;
        public event Action OnTapPerformed;
        public event Action OnDragStarted;
        public event Action OnDragEnded;
        public event Action OnDragHold;

        public void Enable()
        {
            if (_enabled)
                return;

            _enabled = true;

            _pointerPositionAction.action.performed += HandlePointerPositionPerformed;
            _tapStartAction.action.performed += HandleTapStartPerformed;
            _dragCurrentAction.action.started += HandleDragStartPerformed;
            _dragCurrentAction.action.canceled += HandleDragEndPerformed;
            _dragCurrentAction.action.performed += HandleDragCurrentPerformed;

            _pointerPositionAction.action.Enable();
            _tapStartAction.action.Enable();
            _dragCurrentAction.action.Enable();
        }

        public void Disable()
        {
            if (!_enabled)
                return;

            _enabled = false;

            _pointerPositionAction.action.performed -= HandlePointerPositionPerformed;
            _tapStartAction.action.performed -= HandleTapStartPerformed;
            _dragCurrentAction.action.started -= HandleDragStartPerformed;
            _dragCurrentAction.action.canceled -= HandleDragEndPerformed;
            _dragCurrentAction.action.performed -= HandleDragCurrentPerformed;

            _pointerPositionAction.action.Disable();
            _tapStartAction.action.Disable();
            _dragCurrentAction.action.Disable();
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

        private void HandleDragStartPerformed(InputAction.CallbackContext _)
        {
            OnDragStarted?.Invoke();
        }

        private void HandleDragEndPerformed(InputAction.CallbackContext _)
        {
            OnDragEnded?.Invoke();
        }

        private void HandleDragCurrentPerformed(InputAction.CallbackContext _)
        {
            OnDragHold?.Invoke();
        }
    }
}