using UnityEngine;

namespace Game.Core.Input
{
    public enum ActionType
    {
        Press,
        Tap,
        Drag,
    }

    public enum ActionStatus
    {
        Started,
        Performed,
        Canceled
    }

    public struct InputContext
    {
        public ActionType ActionType;
        public ActionStatus ActionStatus;
        public Vector2 ScreenPosition;
        public bool IsOverUI;

        public override string ToString()
        {
            return $"ActionType: {ActionType}, " +
                   $"ActionStatus: {ActionStatus}, " +
                   $"ScreenPosition: {ScreenPosition}, " +
                   $"IsOverUI: {IsOverUI}";
        }
    }
}