using System.Collections.Generic;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Game.Core
{
    public sealed class InputService : MonoBehaviour
    {
        [SerializeField] private InputActionReference _tapAction;
        [SerializeField] private InputActionReference _pointerPositionAction;

        private readonly Subject<Vector2> _onTap = new();
        private readonly List<RaycastResult> _uiRaycastResults = new();

        public Observable<Vector2> OnTap => _onTap;

        private void OnEnable()
        {
            _tapAction.action.performed += HandleTap;

            _pointerPositionAction.action.Enable();
            _tapAction.action.Enable();
        }

        private void OnDisable()
        {
            _tapAction.action.performed -= HandleTap;

            _tapAction.action.Disable();
            _pointerPositionAction.action.Disable();
        }

        private void OnDestroy()
        {
            _onTap.Dispose();
        }

        private void HandleTap(InputAction.CallbackContext _)
        {
            Vector2 screenPosition = _pointerPositionAction.action.ReadValue<Vector2>();

            if (IsPointerOverUI(screenPosition))
                return;

            _onTap.OnNext(screenPosition);
        }

        private bool IsPointerOverUI(Vector2 screenPosition)
        {
            EventSystem eventSystem = EventSystem.current;

            if (eventSystem == null)
                return false;

            var pointerEventData = new PointerEventData(eventSystem)
            {
                position = screenPosition
            };

            _uiRaycastResults.Clear();
            eventSystem.RaycastAll(pointerEventData, _uiRaycastResults);

            return _uiRaycastResults.Count > 0;
        }
    }
}