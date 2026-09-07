using UnityEngine;

namespace Game.Core.Input
{
    public enum ActionType
    {
        TapStarted,
        TapPerformed,
        DragStarted,
        DragEnded,
        DragHold
    }

    public struct InputContext
    {
        public ActionType ActionType;
        public Vector2 ScreenPosition;
        public bool IsOverUI;

        public override string ToString()
        {
            return $"ActionType: {ActionType}, ScreenPosition: {ScreenPosition}, IsOverUI: {IsOverUI}";
        }
    }
}