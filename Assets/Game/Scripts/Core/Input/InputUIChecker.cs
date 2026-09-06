using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Core.Input
{
    public class InputUIChecker
    {
        private readonly List<RaycastResult> _uiRaycastResults = new();

        public bool CheckPointerOverUI(Vector2 screenPosition)
        {
            EventSystem eventSystem = EventSystem.current;

            if (eventSystem == null)
                return false;

            PointerEventData pointerEventData = new(eventSystem)
            {
                position = screenPosition
            };

            _uiRaycastResults.Clear();
            eventSystem.RaycastAll(pointerEventData, _uiRaycastResults);

            return _uiRaycastResults.Count > 0;
        }
    }
}