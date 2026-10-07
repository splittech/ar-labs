using UnityEngine;

namespace Game.Core
{
    public class HorizontalSwipeDetector : IHorizontalSwipeDetector
    {
        private readonly IHorizontalSwipeDetectorView _view;

        public HorizontalSwipeDetector(IHorizontalSwipeDetectorView horizontalSwipeDetectorView)
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