using UnityEngine;

namespace Game.Core
{
    public class TwistDetector
    {
        private readonly GestureServiceView _view;

        private float _accumulatedAngle;
        private bool _detected;

        public TwistDetector(GestureServiceView gestureServiceView)
        {
            _view = gestureServiceView;
        }

        public bool TryDetectTwist(float angleDelta)
        {
            if (_detected)
                return true;

            _accumulatedAngle += angleDelta;
            _detected = Mathf.Abs(_accumulatedAngle) >= _view.MinTwistAngle;

            return _detected;
        }

        public void Reset()
        {
            _accumulatedAngle = 0f;
            _detected = false;
        }
    }
}
