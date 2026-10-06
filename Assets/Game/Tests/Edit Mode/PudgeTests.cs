using Game.Core;
using Game.Gameplay;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode
{
    public class PudgeTests
    {
        public const float InitialPudgeScale = 1f;
        public const float PudgeReduceScaleSpeed = 1f;
        private const float PudgeLinearMovementSpeed = 1f;
        private const float PudgeLinearRotationSpeed = 1f;

        [Test]
        public void Despawn_PudgeIsNotInitialized_PudgeIsDisposed()
        {
            // Arrange.
            Pudge pudge = Setup.Pudge(out var onTick);

            // Act.
            pudge.Despawn();

            // Assert.
            Assert.That(pudge.Disposed, Is.True);
        }

        [Test]
        public void Despawn_PudgeIsInitializedAndDispawnAnimationEnded_PudgeIsDisposed()
        {
            // Arrange.
            Pudge pudge = Setup.Pudge(out var onTick, linearScaleSpeed: PudgeReduceScaleSpeed);
            pudge.Initialize(initialScale: InitialPudgeScale);

            // Act.
            pudge.Despawn();
            onTick.OnNext(new Tick(
                type: TickType.Update,
                deltaTime: InitialPudgeScale / PudgeReduceScaleSpeed
            ));

            // Assert.
            Assert.That(pudge.Disposed, Is.True);
        }

        [Test]
        public void Despawn_PudgeIsInitializedAndInstantAssert_PudgeIsDespawning()
        {
            // Arrange.
            Pudge pudge = Setup.Pudge(out var _);
            pudge.Initialize();

            // Act.
            pudge.Despawn();

            // Assert.
            Assert.That(pudge.IsDespawning, Is.True);
        }

        [Test]
        public void Despawn_PudgeIsInitializedAndInstantAssertAfterDoubleCall_PudgeIsDespawning()
        {
            // Arrange.
            Pudge pudge = Setup.Pudge(out var _);
            pudge.Initialize();

            // Act.
            pudge.Despawn();
            pudge.Despawn();

            // Assert.
            Assert.That(pudge.IsDespawning, Is.True);
        }

        [Test]
        public void MoveTo_PudgeIsNotInitialized_ThrowsException()
        {
            // Arrange.
            Pudge pudge = Setup.Pudge(out var _);

            // Act and Assert.
            Assert.That(() => pudge.MoveTo(Vector3.zero), Throws.Exception);
        }

        [Test]
        public void MoveTo_WithInstantEasingType_PudgeCurrentPositionIsTargetPosition()
        {
            // Arrange.
            Vector3 targetPosition = new(1f, 2f, 3f);

            Pudge pudge = Setup.Pudge(out var _);
            pudge.Initialize();

            // Act.
            pudge.MoveTo(targetPosition: targetPosition, easingType: Pudge.EasingType.Instant);

            // Assert.
            Assert.That(pudge.CurrentPose.position, Is.EqualTo(targetPosition));
        }

        [Test]
        public void MoveTo_WithLinearEasingTypeAndAnimationEnded_PudgeCurrentPositionIsTargetPosition()
        {
            // Arrange.
            Vector3 targetPosition = new(1f, 2f, 3f);

            Pudge pudge = Setup.Pudge(out var onTick, linearMovementSpeed: PudgeLinearMovementSpeed);
            pudge.Initialize();

            float moveDistance = (targetPosition - pudge.CurrentPose.position).magnitude;

            // Act.
            pudge.MoveTo(targetPosition: targetPosition, easingType: Pudge.EasingType.Linear);
            onTick.OnNext(new Tick(
                type: TickType.Update,
                deltaTime: moveDistance / PudgeLinearMovementSpeed
            ));

            // Assert.
            Assert.That(pudge.CurrentPose.position, Is.EqualTo(targetPosition));
        }

        [Test]
        public void RotateTo_PudgeIsNotInitialized_ThrowsException()
        {
            // Arrange.
            Pudge pudge = Setup.Pudge(out var _);

            // Act and Assert.
            Assert.That(() => pudge.RotateTo(Quaternion.identity), Throws.Exception);
        }

        [Test]
        public void RotateTo_WithInstantEasingType_PudgeCurrentRotationIsTargetRotation()
        {
            // Arrange.
            Quaternion targetRotation = new(0.1f, 0.2f, 0.3f, 0.4f);

            Pudge pudge = Setup.Pudge(out var _);
            pudge.Initialize();

            // Act.
            pudge.RotateTo(targetRotation: targetRotation, easingType: Pudge.EasingType.Instant);

            // Assert.
            Assert.That(pudge.CurrentPose.rotation, Is.EqualTo(targetRotation));
        }

        [Test]
        public void RotateTo_WithLinearEasingTypeAndAnimationEnded_PudgeCurrentRotationIsTargetRotation()
        {
            // Arrange.
            Quaternion targetRotation = new(0.1f, 0.2f, 0.3f, 0.4f);

            Pudge pudge = Setup.Pudge(out var onTick, linearMovementSpeed: PudgeLinearMovementSpeed);
            pudge.Initialize();

            float deltaAngle = Quaternion.Angle(pudge.CurrentPose.rotation, targetRotation);

            // Act.
            pudge.RotateTo(targetRotation: targetRotation, easingType: Pudge.EasingType.Linear);
            onTick.OnNext(new Tick(
                type: TickType.Update,
                deltaTime: deltaAngle / PudgeLinearRotationSpeed
            ));

            // Assert.
            Assert.That(pudge.CurrentPose.rotation, Is.EqualTo(targetRotation));
        }
    }
}