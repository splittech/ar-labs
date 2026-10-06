using Game.Core;
using Game.Gameplay;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode
{
    public class PudgeGestureEditorTests
    {
        public const float ScreenWidth = 1000f;
        public const float MaxSwipeRotationAngle = 360f;

        // Свайп на половину ширины экрана даёт половину максимального угла.
        public static readonly Swipe RightHalfScreenSwipe = new(new Vector2(0f, 0f), new Vector2(ScreenWidth / 2f, 0f));
        public static readonly Swipe LeftHalfScreenSwipe = new(new Vector2(ScreenWidth / 2f, 0f), new Vector2(0f, 0f));

        public static readonly Vector2 CrossCenter = new(100f, 100f);

        [Test]
        public void HorizontalSwipe_RightSwipeAndPudgeIsSelected_PudgeRotatesClockwiseByProportionalAngle()
        {
            // Arrange.
            PudgeGestureEditor pudgeGestureEditor = Setup.PudgeGestureEditor(
                out var onHorizontalSwipe, out var selectedPudge,
                screenWidth: ScreenWidth, maxSwipeRotationAngle: MaxSwipeRotationAngle);
            pudgeGestureEditor.Enable();

            Pudge pudge = Setup.Pudge();
            pudge.Initialize();
            selectedPudge.Value = pudge;

            // Act.
            onHorizontalSwipe.OnNext(RightHalfScreenSwipe);

            // Assert.
            Assert.That(pudge.RemainingRotationAngle.CurrentValue, Is.EqualTo(MaxSwipeRotationAngle / 2f));
        }

        [Test]
        public void HorizontalSwipe_LeftSwipeAndPudgeIsSelected_PudgeRotatesCounterClockwiseByProportionalAngle()
        {
            // Arrange.
            PudgeGestureEditor pudgeGestureEditor = Setup.PudgeGestureEditor(
                out var onHorizontalSwipe, out var selectedPudge,
                screenWidth: ScreenWidth, maxSwipeRotationAngle: MaxSwipeRotationAngle);
            pudgeGestureEditor.Enable();

            Pudge pudge = Setup.Pudge();
            pudge.Initialize();
            selectedPudge.Value = pudge;

            // Act.
            onHorizontalSwipe.OnNext(LeftHalfScreenSwipe);

            // Assert.
            Assert.That(pudge.RemainingRotationAngle.CurrentValue, Is.EqualTo(-MaxSwipeRotationAngle / 2f));
        }

        [Test]
        public void HorizontalSwipe_NoPudgeIsSelected_NothingThrows()
        {
            // Arrange.
            PudgeGestureEditor pudgeGestureEditor = Setup.PudgeGestureEditor(out var onHorizontalSwipe, out var _);
            pudgeGestureEditor.Enable();

            // Act and Assert.
            Assert.That(() => onHorizontalSwipe.OnNext(RightHalfScreenSwipe), Throws.Nothing);
        }

        [Test]
        public void HorizontalSwipe_PudgeGestureEditorIsDisabled_PudgeIsNotTransforming()
        {
            // Arrange.
            PudgeGestureEditor pudgeGestureEditor = Setup.PudgeGestureEditor(out var onHorizontalSwipe, out var selectedPudge);
            pudgeGestureEditor.Enable();
            pudgeGestureEditor.Disable();

            Pudge pudge = Setup.Pudge();
            pudge.Initialize();
            selectedPudge.Value = pudge;

            // Act.
            onHorizontalSwipe.OnNext(RightHalfScreenSwipe);

            // Assert.
            Assert.That(pudge.IsTransforming(), Is.False);
        }

        [Test]
        public void Cross_OnNotSelectedPudge_PudgeIsDespawned()
        {
            // Arrange.
            PudgeGestureEditor pudgeGestureEditor = Setup.PudgeGestureEditor(
                out var onCross, out var _, out var raycastService, out var pudgeSpawner);
            pudgeGestureEditor.Enable();

            Pudge pudge = Setup.Pudge(out var _, out var pudgeView);
            pudge.Initialize();
            pudgeView.Pudge.Returns(pudge);

            raycastService
                .TryRaycastOnComponent(Arg.Is(CrossCenter), Arg.Any<LayerMask>(), out Arg.Any<IPudgeView>(), Arg.Any<float>())
                .Returns(call => { call[2] = pudgeView; return true; });

            // Act.
            onCross.OnNext(CrossCenter);

            // Assert.
            pudgeSpawner.Received(1).DespawnPudge(pudge);
        }

        [Test]
        public void Cross_OnSelectedPudge_PudgeIsNotDespawned()
        {
            // Arrange.
            PudgeGestureEditor pudgeGestureEditor = Setup.PudgeGestureEditor(
                out var onCross, out var selectedPudge, out var raycastService, out var pudgeSpawner);
            pudgeGestureEditor.Enable();

            Pudge pudge = Setup.Pudge(out var _, out var pudgeView);
            pudge.Initialize();
            pudgeView.Pudge.Returns(pudge);
            selectedPudge.Value = pudge;

            raycastService
                .TryRaycastOnComponent(Arg.Is(CrossCenter), Arg.Any<LayerMask>(), out Arg.Any<IPudgeView>(), Arg.Any<float>())
                .Returns(call => { call[2] = pudgeView; return true; });

            // Act.
            onCross.OnNext(CrossCenter);

            // Assert.
            pudgeSpawner.DidNotReceive().DespawnPudge(Arg.Any<Pudge>());
        }

        [Test]
        public void Cross_OnTransformingPudge_PudgeIsNotDespawned()
        {
            // Arrange.
            PudgeGestureEditor pudgeGestureEditor = Setup.PudgeGestureEditor(
                out var onCross, out var _, out var raycastService, out var pudgeSpawner);
            pudgeGestureEditor.Enable();

            Pudge pudge = Setup.Pudge(out var _, out var pudgeView);
            pudge.Initialize();
            pudgeView.Pudge.Returns(pudge);
            pudge.MoveTo(Vector3.one, Pudge.EasingType.Linear);

            raycastService
                .TryRaycastOnComponent(Arg.Is(CrossCenter), Arg.Any<LayerMask>(), out Arg.Any<IPudgeView>(), Arg.Any<float>())
                .Returns(call => { call[2] = pudgeView; return true; });

            // Act.
            onCross.OnNext(CrossCenter);

            // Assert.
            pudgeSpawner.DidNotReceive().DespawnPudge(Arg.Any<Pudge>());
        }

        [Test]
        public void Cross_OnEmptySpace_NoPudgeIsDespawned()
        {
            // Arrange.
            PudgeGestureEditor pudgeGestureEditor = Setup.PudgeGestureEditor(
                out var onCross, out var _, out var _, out var pudgeSpawner);
            pudgeGestureEditor.Enable();

            // Act.
            onCross.OnNext(CrossCenter);

            // Assert.
            pudgeSpawner.DidNotReceive().DespawnPudge(Arg.Any<Pudge>());
        }

        [Test]
        public void Cross_PudgeGestureEditorIsDisabled_PudgeIsNotDespawned()
        {
            // Arrange.
            PudgeGestureEditor pudgeGestureEditor = Setup.PudgeGestureEditor(
                out var onCross, out var _, out var raycastService, out var pudgeSpawner);
            pudgeGestureEditor.Enable();
            pudgeGestureEditor.Disable();

            Pudge pudge = Setup.Pudge(out var _, out var pudgeView);
            pudge.Initialize();
            pudgeView.Pudge.Returns(pudge);

            raycastService
                .TryRaycastOnComponent(Arg.Is(CrossCenter), Arg.Any<LayerMask>(), out Arg.Any<IPudgeView>(), Arg.Any<float>())
                .Returns(call => { call[2] = pudgeView; return true; });

            // Act.
            onCross.OnNext(CrossCenter);

            // Assert.
            pudgeSpawner.DidNotReceive().DespawnPudge(Arg.Any<Pudge>());
        }
    }
}
