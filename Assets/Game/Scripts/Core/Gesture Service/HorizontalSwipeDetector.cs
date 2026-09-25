using UnityEngine;

namespace Game.Core
{
    public class HorizontalSwipeDetector
    {
        public bool DetectHorizontalSwipe(Swipe swipe, float maxAngleDelta)
        {
            float rightDeltaAngle = Vector2.Angle(swipe.Vector, Vector2.right);
            float leftDeltaAngle = Vector2.Angle(swipe.Vector, Vector2.left);

            bool rightSwipe = Mathf.Abs(rightDeltaAngle) < maxAngleDelta;
            bool leftSwipe = Mathf.Abs(leftDeltaAngle) < maxAngleDelta;

            return rightSwipe || leftSwipe;
        }
    }
}