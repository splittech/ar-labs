using UnityEngine;

namespace Game.Core
{
    public class CrossDetector
    {
        public bool DetectCross(Swipe firstSwipe, Swipe secondSwipe, float maxAngleDelta, out Vector2 intersection)
        {
            intersection = Vector2.zero;

            bool firstSwipeIsDiagonal = IsDiagonalVector(firstSwipe.Vector, maxAngleDelta);
            bool secondSwipeIsDiagonal = IsDiagonalVector(secondSwipe.Vector, maxAngleDelta);

            if (!firstSwipeIsDiagonal && !secondSwipeIsDiagonal)
                return false;

            Vector2 firstSwipeStartPos = firstSwipe.StartScreenPosition;
            Vector2 firstSwipeEndPos = firstSwipe.EndScreenPosition;
            Vector2 secondSwipeStartPos = secondSwipe.StartScreenPosition;
            Vector2 secondSwipeEndPos = secondSwipe.EndScreenPosition;

            return TryGetIntersection(
                firstSwipeStartPos,
                firstSwipeEndPos,
                secondSwipeStartPos,
                secondSwipeEndPos,
                out intersection);
        }

        private bool IsDiagonalVector(Vector2 vector, float maxAngleDelta)
        {
            Vector2 upLeftDiagonalVector = new(-1f, 1f);
            if (Vector2.Angle(vector, upLeftDiagonalVector) < maxAngleDelta)
                return true;

            Vector2 upRightDiagonalVector = new(1f, 1f);
            if (Vector2.Angle(vector, upRightDiagonalVector) < maxAngleDelta)
                return true;

            Vector2 downRightDiagonalVector = new(1f, -1f);
            if (Vector2.Angle(vector, downRightDiagonalVector) < maxAngleDelta)
                return true;

            Vector2 downLeftDiagonalVector = new(-1f, -1f);
            if (Vector2.Angle(vector, downLeftDiagonalVector) < maxAngleDelta)
                return true;

            return false;
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