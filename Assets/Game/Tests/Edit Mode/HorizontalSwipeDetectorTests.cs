using Game.Core;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode
{
    public class HorizontalSwipeDetectorTests
    {
        public const float MaxHorizontalDeltaAngle = 20f;
        public const float AngleOffset = 1f;
        public const float SwipeLength = 100f;

        [Test]
        public void TryDetectHorizontalSwipe_RightSwipe_ReturnsTrue()
        {
            // Arrange.
            HorizontalSwipeDetector horizontalSwipeDetector = Setup.HorizontalSwipeDetector(MaxHorizontalDeltaAngle);

            // Act.
            bool detected = horizontalSwipeDetector.TryDetectHorizontalSwipe(SwipeAtAngle(0f));

            // Assert.
            Assert.That(detected, Is.True);
        }

        [Test]
        public void TryDetectHorizontalSwipe_LeftSwipe_ReturnsTrue()
        {
            // Arrange.
            HorizontalSwipeDetector horizontalSwipeDetector = Setup.HorizontalSwipeDetector(MaxHorizontalDeltaAngle);

            // Act.
            bool detected = horizontalSwipeDetector.TryDetectHorizontalSwipe(SwipeAtAngle(180f));

            // Assert.
            Assert.That(detected, Is.True);
        }

        [Test]
        public void TryDetectHorizontalSwipe_UpSwipe_ReturnsFalse()
        {
            // Arrange.
            HorizontalSwipeDetector horizontalSwipeDetector = Setup.HorizontalSwipeDetector(MaxHorizontalDeltaAngle);

            // Act.
            bool detected = horizontalSwipeDetector.TryDetectHorizontalSwipe(SwipeAtAngle(90f));

            // Assert.
            Assert.That(detected, Is.False);
        }

        [Test]
        public void TryDetectHorizontalSwipe_DownSwipe_ReturnsFalse()
        {
            // Arrange.
            HorizontalSwipeDetector horizontalSwipeDetector = Setup.HorizontalSwipeDetector(MaxHorizontalDeltaAngle);

            // Act.
            bool detected = horizontalSwipeDetector.TryDetectHorizontalSwipe(SwipeAtAngle(270f));

            // Assert.
            Assert.That(detected, Is.False);
        }

        [Test]
        public void TryDetectHorizontalSwipe_DiagonalSwipe_ReturnsFalse()
        {
            // Arrange.
            HorizontalSwipeDetector horizontalSwipeDetector = Setup.HorizontalSwipeDetector(MaxHorizontalDeltaAngle);

            // Act.
            bool detected = horizontalSwipeDetector.TryDetectHorizontalSwipe(SwipeAtAngle(45f));

            // Assert.
            Assert.That(detected, Is.False);
        }

        [Test]
        public void TryDetectHorizontalSwipe_RightUpSwipeBelowMaxAngle_ReturnsTrue()
        {
            // Arrange.
            HorizontalSwipeDetector horizontalSwipeDetector = Setup.HorizontalSwipeDetector(MaxHorizontalDeltaAngle);

            // Act.
            bool detected = horizontalSwipeDetector.TryDetectHorizontalSwipe(
                SwipeAtAngle(MaxHorizontalDeltaAngle - AngleOffset));

            // Assert.
            Assert.That(detected, Is.True);
        }

        [Test]
        public void TryDetectHorizontalSwipe_RightUpSwipeAboveMaxAngle_ReturnsFalse()
        {
            // Arrange.
            HorizontalSwipeDetector horizontalSwipeDetector = Setup.HorizontalSwipeDetector(MaxHorizontalDeltaAngle);

            // Act.
            bool detected = horizontalSwipeDetector.TryDetectHorizontalSwipe(
                SwipeAtAngle(MaxHorizontalDeltaAngle + AngleOffset));

            // Assert.
            Assert.That(detected, Is.False);
        }

        [Test]
        public void TryDetectHorizontalSwipe_RightDownSwipeBelowMaxAngle_ReturnsTrue()
        {
            // Arrange.
            HorizontalSwipeDetector horizontalSwipeDetector = Setup.HorizontalSwipeDetector(MaxHorizontalDeltaAngle);

            // Act.
            bool detected = horizontalSwipeDetector.TryDetectHorizontalSwipe(
                SwipeAtAngle(-(MaxHorizontalDeltaAngle - AngleOffset)));

            // Assert.
            Assert.That(detected, Is.True);
        }

        [Test]
        public void TryDetectHorizontalSwipe_LeftDownSwipeBelowMaxAngle_ReturnsTrue()
        {
            // Arrange.
            HorizontalSwipeDetector horizontalSwipeDetector = Setup.HorizontalSwipeDetector(MaxHorizontalDeltaAngle);

            // Act.
            bool detected = horizontalSwipeDetector.TryDetectHorizontalSwipe(
                SwipeAtAngle(180f + MaxHorizontalDeltaAngle - AngleOffset));

            // Assert.
            Assert.That(detected, Is.True);
        }

        // Угол отсчитывается от направления вправо против часовой стрелки, как в экранных координатах.
        private static Swipe SwipeAtAngle(float angle)
        {
            Vector2 direction = new(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            return new Swipe(Vector2.zero, direction * SwipeLength);
        }
    }
}
