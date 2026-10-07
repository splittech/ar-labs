using Game.Core;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;

namespace Game.Tests.EditMode
{
    public class CrossDetectorTests
    {
        public static readonly Swipe RisingDiagonalSwipe = new(new Vector2(0f, 0f), new Vector2(100f, 100f));
        public static readonly Swipe FallingDiagonalSwipe = new(new Vector2(0f, 100f), new Vector2(100f, 0f));

        [Test]
        public void TryDetectCross_SingleSwipe_ReturnsFalse()
        {
            // Arrange.
            CrossDetector crossDetector = Setup.CrossDetector();

            // Act.
            bool detected = crossDetector.TryDetectCross(RisingDiagonalSwipe, out var _);

            // Assert.
            Assert.That(detected, Is.False);
        }

        [Test]
        public void TryDetectCross_TwoCrossingDiagonalSwipes_ReturnsTrue()
        {
            // Arrange.
            CrossDetector crossDetector = Setup.CrossDetector();
            crossDetector.TryDetectCross(RisingDiagonalSwipe, out var _);

            // Act.
            bool detected = crossDetector.TryDetectCross(FallingDiagonalSwipe, out var _);

            // Assert.
            Assert.That(detected, Is.True);
        }

        [Test]
        public void TryDetectCross_TwoCrossingDiagonalSwipes_IntersectionIsCrossCenter()
        {
            // Arrange.
            Vector2 crossCenter = new(50f, 50f);

            CrossDetector crossDetector = Setup.CrossDetector();
            crossDetector.TryDetectCross(RisingDiagonalSwipe, out var _);

            // Act.
            crossDetector.TryDetectCross(FallingDiagonalSwipe, out Vector2 intersection);

            // Assert.
            Assert.That(intersection, Is.EqualTo(crossCenter).Using(Vector2EqualityComparer.Instance));
        }

        [Test]
        public void TryDetectCross_TwoParallelDiagonalSwipes_ReturnsFalse()
        {
            // Arrange.
            Swipe parallelSwipe = new(new Vector2(50f, 0f), new Vector2(150f, 100f));

            CrossDetector crossDetector = Setup.CrossDetector();
            crossDetector.TryDetectCross(RisingDiagonalSwipe, out var _);

            // Act.
            bool detected = crossDetector.TryDetectCross(parallelSwipe, out var _);

            // Assert.
            Assert.That(detected, Is.False);
        }

        [Test]
        public void TryDetectCross_DiagonalSwipesDoNotIntersect_ReturnsFalse()
        {
            // Arrange.
            Swipe distantFallingSwipe = new(new Vector2(200f, 100f), new Vector2(300f, 0f));

            CrossDetector crossDetector = Setup.CrossDetector();
            crossDetector.TryDetectCross(RisingDiagonalSwipe, out var _);

            // Act.
            bool detected = crossDetector.TryDetectCross(distantFallingSwipe, out var _);

            // Assert.
            Assert.That(detected, Is.False);
        }

        [Test]
        public void TryDetectCross_HorizontalAndDiagonalSwipes_ReturnsFalse()
        {
            // Arrange.
            Swipe horizontalSwipe = new(new Vector2(0f, 50f), new Vector2(100f, 50f));

            CrossDetector crossDetector = Setup.CrossDetector();
            crossDetector.TryDetectCross(horizontalSwipe, out var _);

            // Act.
            bool detected = crossDetector.TryDetectCross(FallingDiagonalSwipe, out var _);

            // Assert.
            Assert.That(detected, Is.False);
        }

        [Test]
        public void TryDetectCross_SecondSwipeAfterTimerElapsed_ReturnsFalse()
        {
            // Arrange.
            CrossDetector crossDetector = Setup.CrossDetector(out var timerElapsed);
            crossDetector.TryDetectCross(RisingDiagonalSwipe, out var _);

            // Act.
            timerElapsed.Value = true;
            bool detected = crossDetector.TryDetectCross(FallingDiagonalSwipe, out var _);

            // Assert.
            Assert.That(detected, Is.False);
        }

        [Test]
        public void TryDetectCross_ThirdSwipeAfterDetectedCross_ReturnsFalse()
        {
            // Arrange.
            CrossDetector crossDetector = Setup.CrossDetector();
            crossDetector.TryDetectCross(RisingDiagonalSwipe, out var _);
            crossDetector.TryDetectCross(FallingDiagonalSwipe, out var _);

            // Act.
            bool detected = crossDetector.TryDetectCross(RisingDiagonalSwipe, out var _);

            // Assert.
            Assert.That(detected, Is.False);
        }
    }
}
