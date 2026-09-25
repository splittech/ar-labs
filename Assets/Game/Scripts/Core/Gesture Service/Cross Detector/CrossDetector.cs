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

            if (!_crossTimer.Elapsed.CurrentValue)
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

            // Векторы направлений отрезков
            Vector2 r = b - a;
            Vector2 s = d - c;

            // Знаменатель (векторное произведение направлений)
            float denominator = CrossProduct2D(r, s);

            // Направление между начальными точками
            Vector2 cMinusA = c - a;

            // Если знаменатель равен 0, отрезки параллельны или лежат на одной прямой
            if (Mathf.Approximately(denominator, 0f))
            {
                // Здесь отрезки либо не пересекаются, либо накладываются друг на друга.
                // Для простоты большинства игровых задач считаем, что четкой точки пересечения нет.
                return false;
            }

            // Параметры t и u для линейных уравнений отрезков
            float t = CrossProduct2D(cMinusA, s) / denominator;
            float u = CrossProduct2D(cMinusA, r) / denominator;

            // Отрезки пересекаются только если t и u находятся в диапазоне от 0 до 1
            if (t >= 0f && t <= 1f && u >= 0f && u <= 1f)
            {
                // Вычисляем точку пересечения на основе первого отрезка
                intersection = a + t * r;
                return true;
            }

            // Отрезки не параллельны, но их воображаемые продолжения (прямые) пересекаются за пределами длин самих отрезков
            return false;
        }

        private float CrossProduct2D(Vector2 v1, Vector2 v2)
        {
            return v1.x * v2.y - v1.y * v2.x;
        }
    }
}