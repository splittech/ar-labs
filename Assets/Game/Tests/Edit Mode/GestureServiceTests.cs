using Game.Core;
using NSubstitute;
using NUnit.Framework;
using R3;
using UnityEngine;

namespace Game.Tests.EditMode
{
    public class GestureServiceTests
    {
        public static readonly Swipe AnySwipe = new(new Vector2(0f, 0f), new Vector2(100f, 100f));
        public static readonly Vector2 CrossCenter = new(50f, 50f);

        [Test]
        public void Swipe_HorizontalSwipeDetected_OnHorizontalSwipeIsInvoked()
        {
            // Arrange.
            GestureService gestureService = Setup.GestureService(out var onSwipe, out var horizontalSwipeDetector, out var _);
            gestureService.Enable();

            horizontalSwipeDetector.TryDetectHorizontalSwipe(Arg.Any<Swipe>()).Returns(true);

            bool horizontalSwipeInvoked = false;
            gestureService.OnHorizontalSwipe.Subscribe(_ => horizontalSwipeInvoked = true);

            // Act.
            onSwipe.OnNext(AnySwipe);

            // Assert.
            Assert.That(horizontalSwipeInvoked, Is.True);
        }

        [Test]
        public void Swipe_HorizontalSwipeDetected_SwipeIsNotPassedToCrossDetector()
        {
            // Arrange.
            GestureService gestureService = Setup.GestureService(out var onSwipe, out var horizontalSwipeDetector, out var crossDetector);
            gestureService.Enable();

            horizontalSwipeDetector.TryDetectHorizontalSwipe(Arg.Any<Swipe>()).Returns(true);

            // Act.
            onSwipe.OnNext(AnySwipe);

            // Assert.
            crossDetector.DidNotReceive().TryDetectCross(Arg.Any<Swipe>(), out Arg.Any<Vector2>());
        }

        [Test]
        public void Swipe_HorizontalSwipeNotDetected_SwipeIsPassedToCrossDetector()
        {
            // Arrange.
            GestureService gestureService = Setup.GestureService(out var onSwipe, out var _, out var crossDetector);
            gestureService.Enable();

            // Act.
            onSwipe.OnNext(AnySwipe);

            // Assert.
            crossDetector.Received(1).TryDetectCross(Arg.Is(AnySwipe), out Arg.Any<Vector2>());
        }

        [Test]
        public void Swipe_CrossDetected_OnCrossIsInvokedWithIntersection()
        {
            // Arrange.
            GestureService gestureService = Setup.GestureService(out var onSwipe, out var _, out var crossDetector);
            gestureService.Enable();

            crossDetector
                .TryDetectCross(Arg.Any<Swipe>(), out Arg.Any<Vector2>())
                .Returns(call => { call[1] = CrossCenter; return true; });

            Vector2? crossCenter = null;
            gestureService.OnCross.Subscribe(center => crossCenter = center);

            // Act.
            onSwipe.OnNext(AnySwipe);

            // Assert.
            Assert.That(crossCenter, Is.EqualTo(CrossCenter));
        }

        [Test]
        public void Swipe_NoGestureDetected_NoGestureEventIsInvoked()
        {
            // Arrange.
            GestureService gestureService = Setup.GestureService(out var onSwipe, out var _, out var _);
            gestureService.Enable();

            bool gestureInvoked = false;
            gestureService.OnHorizontalSwipe.Subscribe(_ => gestureInvoked = true);
            gestureService.OnCross.Subscribe(_ => gestureInvoked = true);

            // Act.
            onSwipe.OnNext(AnySwipe);

            // Assert.
            Assert.That(gestureInvoked, Is.False);
        }

        [Test]
        public void Swipe_GestureServiceIsDisabled_SwipeIsNotDetected()
        {
            // Arrange.
            GestureService gestureService = Setup.GestureService(out var onSwipe, out var horizontalSwipeDetector, out var _);
            gestureService.Enable();
            gestureService.Disable();

            // Act.
            onSwipe.OnNext(AnySwipe);

            // Assert.
            horizontalSwipeDetector.DidNotReceive().TryDetectHorizontalSwipe(Arg.Any<Swipe>());
        }

        [Test]
        public void Enable_CalledTwice_SwipeIsDetectedOnce()
        {
            // Arrange.
            GestureService gestureService = Setup.GestureService(out var onSwipe, out var horizontalSwipeDetector, out var _);

            // Act.
            gestureService.Enable();
            gestureService.Enable();
            onSwipe.OnNext(AnySwipe);

            // Assert.
            horizontalSwipeDetector.Received(1).TryDetectHorizontalSwipe(Arg.Any<Swipe>());
        }

        [Test]
        public void Enable_AfterDisable_SwipeIsDetected()
        {
            // Arrange.
            GestureService gestureService = Setup.GestureService(out var onSwipe, out var horizontalSwipeDetector, out var _);
            gestureService.Enable();
            gestureService.Disable();

            // Act.
            gestureService.Enable();
            onSwipe.OnNext(AnySwipe);

            // Assert.
            horizontalSwipeDetector.Received(1).TryDetectHorizontalSwipe(Arg.Any<Swipe>());
        }
    }
}
