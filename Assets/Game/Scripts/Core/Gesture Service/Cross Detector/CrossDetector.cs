using UnityEngine;

namespace Game.Core
{
    public class CrossDetector
    {
        private readonly CrossDetectorView _crossDetectorView;
        private readonly Timer _crossTimer;
        private Swipe _previousSwipe;

        public CrossDetector(CrossDetectorView crossDetectorView, TimerService timerService)
        {
            _crossDetectorView = crossDetectorView;

            _crossTimer = timerService.CreateTimer();
        }

        public bool TryDetectCross(Swipe newSwipe, out Vector2 intersection)
        {
            intersection = Vector2.zero;

            if (_crossTimer.Elapsed.CurrentValue)
            {
                CastAwayPreviousSwipe(newSwipe);
                return false;
            }

            bool firstSwipeIsDiagonal = IsDiagonalVector(_previousSwipe.Vector, _crossDetectorView.MaxDiagonalDeltaAngle);
            bool secondSwipeIsDiagonal = IsDiagonalVector(newSwipe.Vector, _crossDetectorView.MaxDiagonalDeltaAngle);

            if (!firstSwipeIsDiagonal || !secondSwipeIsDiagonal)
            {
                CastAwayPreviousSwipe(newSwipe);
                return false;
            }

            bool crossDetected = TryGetIntersection(
                _previousSwipe.StartScreenPosition,
                _previousSwipe.EndScreenPosition,
                newSwipe.StartScreenPosition,
                newSwipe.EndScreenPosition,
                out intersection);

            if (!crossDetected)
            {
                CastAwayPreviousSwipe(newSwipe);
                return false;
            }

            _crossTimer.Stop();
            return true;
        }

        private void CastAwayPreviousSwipe(Swipe newSwipe)
        {
            _crossTimer.Reset(_crossDetectorView.MaxDeltaTimeBwetweenTwoSwipes);
            _previousSwipe = newSwipe;
        }

        private bool IsDiagonalVector(Vector2 vector, float maxAngleDelta)
        {
            float upDeltaAngle = Vector2.Angle(vector, Vector2.up);

            bool isUpDiagonalVector = Mathf.Abs(Mathf.DeltaAngle(upDeltaAngle, 45f)) < maxAngleDelta;
            bool isDownDiagonalVector = Mathf.Abs(Mathf.DeltaAngle(upDeltaAngle, 135f)) < maxAngleDelta;

            return isUpDiagonalVector || isDownDiagonalVector;
        }

        private bool TryGetIntersection(Vector2 a, Vector2 b, Vector2 c, Vector2 d, out Vector2 intersection)
        {
            intersection = Vector2.zero;

            Vector2 r = b - a;
            Vector2 s = d - c;

            float directionsCross = CrossProduct2D(r, s);

            if (Mathf.Approximately(directionsCross, 0f))
                return false;

            // a + t * r = c + u * s
            // Cross(a + t*r, s) = Cross(c + u*s, s)
            // Cross(a, s) + t * Cross(r, s) = Cross(c, s) + u * Cross(s, s)
            // Cross(a, s) + t * Cross(r, s) = Cross(c, s)
            // t = (Cross(c, s) - Cross(a, s)) / Cross(r, s)
            // t = Cross(c - a, s) / Cross(r, s)
            float t = CrossProduct2D(c - a, s) / directionsCross;
            float u = CrossProduct2D(c - a, r) / directionsCross;

            if (t > 0f && t < 1f && u > 0f && u < 1f)
            {
                intersection = a + t * r;
                return true;
            }

            return false;
        }

        private float CrossProduct2D(Vector2 v1, Vector2 v2)
        {
            return v1.x * v2.y - v1.y * v2.x;
        }
    }
}