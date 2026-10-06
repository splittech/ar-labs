using UnityEngine;

namespace Game.Core
{
    public class CrossDetector
    {
        private readonly ICrossDetectorView _view;
        private readonly ITimer _crossTimer;

        private Swipe _previousSwipe;
        private bool _hasPreviousSwipe;

        public CrossDetector(ICrossDetectorView crossDetectorView, ITimerService timerService)
        {
            _view = crossDetectorView;

            _crossTimer = timerService.CreateTimer();
        }

        public bool TryDetectCross(Swipe newSwipe)
        {
            return TryDetectCross(newSwipe, out var _);
        }

        public bool TryDetectCross(Swipe newSwipe, out Vector2 intersection)
        {
            intersection = Vector2.zero;

            bool withinTimeWindow = _hasPreviousSwipe && !_crossTimer.Elapsed.CurrentValue;

            if (withinTimeWindow && IsCross(_previousSwipe, newSwipe, out intersection))
            {
                _hasPreviousSwipe = false;
                _crossTimer.Stop();
                return true;
            }

            _previousSwipe = newSwipe;
            _hasPreviousSwipe = true;
            _crossTimer.Reset(_view.MaxDeltaTimeBwetweenTwoSwipes);
            return false;
        }

        private bool IsCross(Swipe first, Swipe second, out Vector2 intersection)
        {
            intersection = Vector2.zero;

            float maxDelta = _view.MaxDiagonalDeltaAngle;
            if (!IsDiagonal(first.Vector, maxDelta) || !IsDiagonal(second.Vector, maxDelta))
                return false;

            if (DiagonalType(first.Vector) == DiagonalType(second.Vector))
                return false;

            return TryGetIntersection(
                first.StartScreenPosition, first.EndScreenPosition,
                second.StartScreenPosition, second.EndScreenPosition,
                out intersection);
        }

        private bool IsDiagonal(Vector2 vector, float maxAngleDelta)
        {
            float diagonalAngle = Mathf.Atan2(Mathf.Abs(vector.y), Mathf.Abs(vector.x)) * Mathf.Rad2Deg;
            return Mathf.Abs(diagonalAngle - 45f) < maxAngleDelta;
        }

        private bool DiagonalType(Vector2 vector)
        {
            return vector.x * vector.y > 0f;
        }

        private bool TryGetIntersection(Vector2 a, Vector2 b, Vector2 c, Vector2 d, out Vector2 intersection)
        {
            intersection = Vector2.zero;

            Vector2 r = b - a;
            Vector2 s = d - c;

            float directionsCross = CrossProduct2D(r, s);

            if (Mathf.Approximately(directionsCross, 0f))
                return false;

            // a + t * r = c + u * s <= find t and u
            // Cross(a + t*r, s) = Cross(c + u*s, s)
            // Cross(a, s) + t * Cross(r, s) = Cross(c, s) + u * Cross(s, s)
            // Cross(a, s) + t * Cross(r, s) = Cross(c, s)
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