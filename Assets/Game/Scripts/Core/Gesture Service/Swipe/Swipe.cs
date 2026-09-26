using UnityEngine;

namespace Game.Core
{
    public readonly struct Swipe
    {
        public readonly Vector2 StartScreenPosition;
        public readonly Vector2 EndScreenPosition;
        public readonly Vector2 Vector;

        public Swipe(Vector2 startScreenPosition, Vector2 endScreenPosition)
        {
            StartScreenPosition = startScreenPosition;
            EndScreenPosition = endScreenPosition;

            Vector = endScreenPosition - startScreenPosition;
        }

        public override string ToString()
        {
            return $"StartScreenPosition: {StartScreenPosition}, " +
                   $"EndScreenPosition: {EndScreenPosition}, " +
                   $"Vector: {Vector}";
        }
    }
}