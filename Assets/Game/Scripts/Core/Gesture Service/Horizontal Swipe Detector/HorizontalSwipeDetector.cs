using UnityEngine;

namespace Game.Core
{
    public class HorizontalSwipeDetector
    {
        private readonly HorizontalSwipeDetectorView _view;

        public HorizontalSwipeDetector(HorizontalSwipeDetectorView horizontalSwipeDetectorView)
        {
            _view = horizontalSwipeDetectorView;
        }

        public bool TryDetectHorizontalSwipe(Swipe swipe)
        {
            float upDeltaAngle = Vector2.Angle(swipe.Vector, Vector2.up);
            bool isHorizontalVector = Mathf.Abs(Mathf.DeltaAngle(upDeltaAngle, 90f)) < _view.MaxHorizontalDeltaAngle;
            return isHorizontalVector;
        }
    }
}