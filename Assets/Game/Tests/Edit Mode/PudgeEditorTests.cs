using Game.Core.Input;
using Game.Gameplay;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode
{
    public class PudgeEditorTests
    {
        public const float InitialPudgeScale = 1f;
        public const float ScaleDelta = 0.5f;
        public const float RotationDelta = 90f;

        public static readonly Vector2 FirstPudgeScreenPosition = new(100f, 100f);
        public static readonly Vector2 SecondPudgeScreenPosition = new(300f, 300f);
        public static readonly Vector2 EmptyScreenPosition = new(500f, 500f);

        public static readonly InputContext FirstPudgeTap = new()
        {
            ActionType = ActionType.Tap,
            ActionStatus = ActionStatus.Performed,
            ScreenPosition = FirstPudgeScreenPosition,
            IsOverUI = false
        };

        public static readonly InputContext SecondPudgeTap = new()
        {
            ActionType = ActionType.Tap,
            ActionStatus = ActionStatus.Performed,
            ScreenPosition = SecondPudgeScreenPosition,
            IsOverUI = false
        };

        public static readonly InputContext EmptySpaceTap = new()
        {
            ActionType = ActionType.Tap,
            ActionStatus = ActionStatus.Performed,
            ScreenPosition = EmptyScreenPosition,
            IsOverUI = false
        };

        public static readonly InputContext FirstPudgeTapOverUI = new()
        {
            ActionType = ActionType.Tap,
            ActionStatus = ActionStatus.Performed,
            ScreenPosition = FirstPudgeScreenPosition,
            IsOverUI = true
        };

        [Test]
        public void Tap_OnPudge_PudgeIsSelected()
        {
            // Arrange.
            PudgeEditor pudgeEditor = Setup.PudgeEditor(out var onInputActionPerformed, out var raycastService);
            pudgeEditor.Enable();

            Pudge pudge = Setup.Pudge(out var _, out var pudgeView);
            pudge.Initialize();
            pudgeView.Pudge.Returns(pudge);

            raycastService
                .TryRaycastOnComponent(Arg.Is(FirstPudgeScreenPosition), Arg.Any<LayerMask>(), out Arg.Any<IPudgeView>(), Arg.Any<float>())
                .Returns(call => { call[2] = pudgeView; return true; });

            // Act.
            onInputActionPerformed.OnNext(FirstPudgeTap);

            // Assert.
            Assert.That(pudgeEditor.SelectedPudge.CurrentValue, Is.EqualTo(pudge));
        }

        [Test]
        public void Tap_OnPudgeOverUI_PudgeIsNotSelected()
        {
            // Arrange.
            PudgeEditor pudgeEditor = Setup.PudgeEditor(out var onInputActionPerformed, out var raycastService);
            pudgeEditor.Enable();

            Pudge pudge = Setup.Pudge(out var _, out var pudgeView);
            pudge.Initialize();
            pudgeView.Pudge.Returns(pudge);

            raycastService
                .TryRaycastOnComponent(Arg.Is(FirstPudgeScreenPosition), Arg.Any<LayerMask>(), out Arg.Any<IPudgeView>(), Arg.Any<float>())
                .Returns(call => { call[2] = pudgeView; return true; });

            // Act.
            onInputActionPerformed.OnNext(FirstPudgeTapOverUI);

            // Assert.
            Assert.That(pudgeEditor.SelectedPudge.CurrentValue, Is.Null);
        }

        [Test]
        public void Tap_OnEmptySpaceAfterPudgeSelected_SelectedPudgeIsNull()
        {
            // Arrange.
            PudgeEditor pudgeEditor = Setup.PudgeEditor(out var onInputActionPerformed, out var raycastService);
            pudgeEditor.Enable();

            Pudge pudge = Setup.Pudge(out var _, out var pudgeView);
            pudge.Initialize();
            pudgeView.Pudge.Returns(pudge);

            raycastService
                .TryRaycastOnComponent(Arg.Is(FirstPudgeScreenPosition), Arg.Any<LayerMask>(), out Arg.Any<IPudgeView>(), Arg.Any<float>())
                .Returns(call => { call[2] = pudgeView; return true; });

            onInputActionPerformed.OnNext(FirstPudgeTap);

            // Act.
            onInputActionPerformed.OnNext(EmptySpaceTap);

            // Assert.
            Assert.That(pudgeEditor.SelectedPudge.CurrentValue, Is.Null);
        }

        [Test]
        public void Tap_OnOtherPudge_FirstPudgeIsDeselected()
        {
            // Arrange.
            PudgeEditor pudgeEditor = Setup.PudgeEditor(out var onInputActionPerformed, out var raycastService);
            pudgeEditor.Enable();

            Pudge firstPudge = Setup.Pudge(out var _, out var firstPudgeView);
            firstPudge.Initialize();
            firstPudgeView.Pudge.Returns(firstPudge);

            Pudge secondPudge = Setup.Pudge(out var _, out var secondPudgeView);
            secondPudge.Initialize();
            secondPudgeView.Pudge.Returns(secondPudge);

            raycastService
                .TryRaycastOnComponent(Arg.Is(FirstPudgeScreenPosition), Arg.Any<LayerMask>(), out Arg.Any<IPudgeView>(), Arg.Any<float>())
                .Returns(call => { call[2] = firstPudgeView; return true; });
            raycastService
                .TryRaycastOnComponent(Arg.Is(SecondPudgeScreenPosition), Arg.Any<LayerMask>(), out Arg.Any<IPudgeView>(), Arg.Any<float>())
                .Returns(call => { call[2] = secondPudgeView; return true; });

            onInputActionPerformed.OnNext(FirstPudgeTap);

            // Act.
            onInputActionPerformed.OnNext(SecondPudgeTap);

            // Assert.
            Assert.That(firstPudge.Selected, Is.False);
        }

        [Test]
        public void Tap_OnOtherPudge_PreviousSelectedPudgeIsFirstPudge()
        {
            // Arrange.
            PudgeEditor pudgeEditor = Setup.PudgeEditor(out var onInputActionPerformed, out var raycastService);
            pudgeEditor.Enable();

            Pudge firstPudge = Setup.Pudge(out var _, out var firstPudgeView);
            firstPudge.Initialize();
            firstPudgeView.Pudge.Returns(firstPudge);

            Pudge secondPudge = Setup.Pudge(out var _, out var secondPudgeView);
            secondPudge.Initialize();
            secondPudgeView.Pudge.Returns(secondPudge);

            raycastService
                .TryRaycastOnComponent(Arg.Is(FirstPudgeScreenPosition), Arg.Any<LayerMask>(), out Arg.Any<IPudgeView>(), Arg.Any<float>())
                .Returns(call => { call[2] = firstPudgeView; return true; });
            raycastService
                .TryRaycastOnComponent(Arg.Is(SecondPudgeScreenPosition), Arg.Any<LayerMask>(), out Arg.Any<IPudgeView>(), Arg.Any<float>())
                .Returns(call => { call[2] = secondPudgeView; return true; });

            onInputActionPerformed.OnNext(FirstPudgeTap);

            // Act.
            onInputActionPerformed.OnNext(SecondPudgeTap);

            // Assert.
            Assert.That(pudgeEditor.PreviousSelectedPudge.CurrentValue, Is.EqualTo(firstPudge));
        }

        [Test]
        public void AddScale_PudgeIsSelected_PudgeScaleIncreasedByScaleDelta()
        {
            // Arrange.
            PudgeEditor pudgeEditor = Setup.PudgeEditor(out var onInputActionPerformed, out var raycastService, scaleDelta: ScaleDelta);
            pudgeEditor.Enable();

            Pudge pudge = Setup.Pudge(out var _, out var pudgeView);
            pudge.Initialize(initialScale: InitialPudgeScale);
            pudgeView.Pudge.Returns(pudge);

            raycastService
                .TryRaycastOnComponent(Arg.Is(FirstPudgeScreenPosition), Arg.Any<LayerMask>(), out Arg.Any<IPudgeView>(), Arg.Any<float>())
                .Returns(call => { call[2] = pudgeView; return true; });

            onInputActionPerformed.OnNext(FirstPudgeTap);

            // Act.
            pudgeEditor.AddScale();

            // Assert.
            Assert.That(pudge.CurrentScale, Is.EqualTo(InitialPudgeScale + ScaleDelta));
        }

        [Test]
        public void SubstractScale_ScaleBecomesZero_PudgeScaleIsNotChanged()
        {
            // Arrange.
            PudgeEditor pudgeEditor = Setup.PudgeEditor(out var onInputActionPerformed, out var raycastService, scaleDelta: ScaleDelta);
            pudgeEditor.Enable();

            Pudge pudge = Setup.Pudge(out var _, out var pudgeView);
            pudge.Initialize(initialScale: ScaleDelta);
            pudgeView.Pudge.Returns(pudge);

            raycastService
                .TryRaycastOnComponent(Arg.Is(FirstPudgeScreenPosition), Arg.Any<LayerMask>(), out Arg.Any<IPudgeView>(), Arg.Any<float>())
                .Returns(call => { call[2] = pudgeView; return true; });

            onInputActionPerformed.OnNext(FirstPudgeTap);

            // Act.
            pudgeEditor.SubstractScale();

            // Assert.
            Assert.That(pudge.CurrentScale, Is.EqualTo(ScaleDelta));
        }

        [Test]
        public void RotateClockwise_PudgeIsSelected_TotalAngleDeltaIsRotationDelta()
        {
            // Arrange.
            PudgeEditor pudgeEditor = Setup.PudgeEditor(out var onInputActionPerformed, out var raycastService, rotationDelta: RotationDelta);
            pudgeEditor.Enable();

            Pudge pudge = Setup.Pudge(out var _, out var pudgeView);
            pudge.Initialize();
            pudgeView.Pudge.Returns(pudge);

            raycastService
                .TryRaycastOnComponent(Arg.Is(FirstPudgeScreenPosition), Arg.Any<LayerMask>(), out Arg.Any<IPudgeView>(), Arg.Any<float>())
                .Returns(call => { call[2] = pudgeView; return true; });

            onInputActionPerformed.OnNext(FirstPudgeTap);

            // Act.
            pudgeEditor.RotateClockwise();

            // Assert.
            Assert.That(pudgeEditor.TotalAngleDelta.CurrentValue, Is.EqualTo(RotationDelta));
        }

        [Test]
        public void ResetScaleAndRotation_ScaleWasChanged_PudgeScaleIsInitial()
        {
            // Arrange.
            PudgeEditor pudgeEditor = Setup.PudgeEditor(out var onInputActionPerformed, out var raycastService, scaleDelta: ScaleDelta);
            pudgeEditor.Enable();

            Pudge pudge = Setup.Pudge(out var _, out var pudgeView);
            pudge.Initialize(initialScale: InitialPudgeScale);
            pudgeView.Pudge.Returns(pudge);

            raycastService
                .TryRaycastOnComponent(Arg.Is(FirstPudgeScreenPosition), Arg.Any<LayerMask>(), out Arg.Any<IPudgeView>(), Arg.Any<float>())
                .Returns(call => { call[2] = pudgeView; return true; });

            onInputActionPerformed.OnNext(FirstPudgeTap);
            pudgeEditor.AddScale();
            pudgeEditor.AddScale();

            // Act.
            pudgeEditor.ResetScaleAndRotation();

            // Assert.
            Assert.That(pudge.CurrentScale, Is.EqualTo(InitialPudgeScale));
        }

        [Test]
        public void Disable_PudgeIsSelected_PudgeIsDeselected()
        {
            // Arrange.
            PudgeEditor pudgeEditor = Setup.PudgeEditor(out var onInputActionPerformed, out var raycastService);
            pudgeEditor.Enable();

            Pudge pudge = Setup.Pudge(out var _, out var pudgeView);
            pudge.Initialize();
            pudgeView.Pudge.Returns(pudge);

            raycastService
                .TryRaycastOnComponent(Arg.Is(FirstPudgeScreenPosition), Arg.Any<LayerMask>(), out Arg.Any<IPudgeView>(), Arg.Any<float>())
                .Returns(call => { call[2] = pudgeView; return true; });

            onInputActionPerformed.OnNext(FirstPudgeTap);

            // Act.
            pudgeEditor.Disable();

            // Assert.
            Assert.That(pudge.Selected, Is.False);
        }
    }
}
