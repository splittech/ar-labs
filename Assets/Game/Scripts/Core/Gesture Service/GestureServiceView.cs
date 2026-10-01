using System.Collections.Generic;
using Lean.Touch;
using R3;
using UnityEngine;

namespace Game.Core
{
    public class GestureServiceView : MonoBehaviour
    {
        private const int TwistFingerCount = 2;

        [Header("Twist")]
        [SerializeField] private float _minTwistAngle = 10f;

        private readonly List<LeanFinger> _twistFingers = new(TwistFingerCount);

        // Взводится, как только на экране оказалось больше одного пальца,
        // и сбрасывается, когда подняты все пальцы. Пока взведён, тапы и свайпы игнорируются.
        private bool _multiTouch;
        private bool _twisting;

        private Subject<Swipe> _onSwipe = new();
        public Observable<Swipe> OnSwipe => _onSwipe;

        private Subject<LeanFinger> _onTap = new();
        public Observable<LeanFinger> OnTap => _onTap;

        private Subject<float> _onTwist = new();
        public Observable<float> OnTwist => _onTwist;

        private Subject<Unit> _onTwistEnded = new();
        public Observable<Unit> OnTwistEnded => _onTwistEnded;

        public float MinTwistAngle => _minTwistAngle;

        private void OnEnable()
        {
            LeanTouch.OnFingerSwipe += OnFingerSwipe;
            LeanTouch.OnFingerTap += OnFingerTap;
            LeanTouch.OnGesture += OnGesture;
        }

        private void OnDisable()
        {
            LeanTouch.OnFingerSwipe -= OnFingerSwipe;
            LeanTouch.OnFingerTap -= OnFingerTap;
            LeanTouch.OnGesture -= OnGesture;
        }

        private void OnFingerSwipe(LeanFinger leanFinger)
        {
            if (_multiTouch || leanFinger.StartedOverGui)
                return;

            Swipe swipe = new(leanFinger.StartScreenPosition, leanFinger.LastScreenPosition);
            _onSwipe.OnNext(swipe);
        }

        private void OnFingerTap(LeanFinger leanFinger)
        {
            if (_multiTouch || leanFinger.StartedOverGui)
                return;

            _onTap.OnNext(leanFinger);
        }

        // LeanTouch вызывает OnGesture после событий тапа и свайпа этого же кадра,
        // поэтому палец, поднятый последним, ещё попадает под фильтр мультитача.
        private void OnGesture(List<LeanFinger> fingers)
        {
            _twistFingers.Clear();
            int pressedCount = 0;

            foreach (LeanFinger finger in fingers)
            {
                if (finger.Index == LeanTouch.HOVER_FINGER_INDEX)
                    continue;

                if (finger.Set)
                    pressedCount++;

                if (finger.Set && !finger.StartedOverGui)
                    _twistFingers.Add(finger);
            }

            if (pressedCount > 1)
                _multiTouch = true;

            if (_twistFingers.Count == TwistFingerCount && pressedCount == TwistFingerCount)
            {
                _twisting = true;
                _onTwist.OnNext(LeanGesture.GetTwistDegrees(_twistFingers));
            }
            else if (_twisting)
            {
                _twisting = false;
                _onTwistEnded.OnNext(Unit.Default);
            }

            if (pressedCount == 0)
                _multiTouch = false;
        }
    }
}
