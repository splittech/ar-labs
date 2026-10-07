using System;
using Game.Core;
using Game.Gameplay;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools.Utils;

namespace Game.Tests.EditMode
{
    public class PudgeTests
    {
        public const float InitialPudgeScale = 1f;
        public const float PudgeReduceScaleSpeed = 1f;
        public const float PudgeLinearMovementSpeed = 1f;
        public const float PudgeLinearRotationSpeed = 1f;

        [Test]
        public void Despawn_PudgeIsNotInitialized_PudgeIsDisposed()
        {
            // Arrange.
            Pudge pudge = Setup.Pudge();

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
            Pudge pudge = Setup.Pudge();
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
            Pudge pudge = Setup.Pudge();
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
            Pudge pudge = Setup.Pudge();

            // Act and Assert.
            Assert.That(() => pudge.MoveTo(Vector3.zero), Throws.Exception);
        }

        [Test]
        public void MoveTo_WithInstantEasingType_PudgeCurrentPositionIsTargetPosition()
        {
            // Arrange.
            Vector3 targetPosition = new(1f, 2f, 3f);

            Pudge pudge = Setup.Pudge();
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
            Pudge pudge = Setup.Pudge();

            // Act and Assert.
            Assert.That(() => pudge.RotateTo(Quaternion.identity), Throws.Exception);
        }

        [Test]
        public void RotateTo_WithInstantEasingType_PudgeCurrentRotationIsTargetRotation()
        {
            // Arrange.
            Quaternion targetRotation = Quaternion.Euler(0f, 45f, 0f);

            Pudge pudge = Setup.Pudge();
            pudge.Initialize();

            // Act.
            pudge.RotateTo(targetRotation: targetRotation, easingType: Pudge.EasingType.Instant);

            // Assert.
            Assert.That(pudge.CurrentPose.rotation, Is.EqualTo(targetRotation).Using(QuaternionEqualityComparer.Instance));
        }

        [Test]
        public void RotateTo_WithLinearEasingTypeAndAnimationEnded_PudgeCurrentRotationIsTargetRotation()
        {
            // Arrange.
            Quaternion targetRotation = Quaternion.Euler(0f, 45f, 0f);

            Pudge pudge = Setup.Pudge(out var onTick, linearRotationSpeed: PudgeLinearRotationSpeed);
            pudge.Initialize();

            float deltaAngle = Mathf.Abs(Mathf.DeltaAngle(
                pudge.CurrentPose.rotation.eulerAngles.y,
                targetRotation.eulerAngles.y));

            // Act.
            pudge.RotateTo(targetRotation: targetRotation, easingType: Pudge.EasingType.Linear);
            onTick.OnNext(new Tick(
                type: TickType.Update,
                deltaTime: deltaAngle / PudgeLinearRotationSpeed
            ));

            // Assert.
            Assert.That(pudge.CurrentPose.rotation, Is.EqualTo(targetRotation).Using(QuaternionEqualityComparer.Instance));
        }

        [Test]
        public void Initialize_CalledTwice_PudgeKeepsFirstPose()
        {
            // Arrange.
            Pose firstPose = new(new Vector3(1f, 2f, 3f), Quaternion.identity);
            Pose secondPose = new(new Vector3(4f, 5f, 6f), Quaternion.identity);

            Pudge pudge = Setup.Pudge();

            // Act.
            pudge.Initialize(initialPose: firstPose);
            pudge.Initialize(initialPose: secondPose);

            // Assert.
            Assert.That(pudge.CurrentPose.position, Is.EqualTo(firstPose.position));
        }

        [Test]
        public void MoveTo_PudgeIsDespawning_ThrowsInvalidOperationException()
        {
            // Arrange.
            Pudge pudge = Setup.Pudge();
            pudge.Initialize();
            pudge.Despawn();

            // Act and Assert.
            Assert.That(() => pudge.MoveTo(Vector3.one), Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void MoveTo_PudgeIsDisposed_ThrowsObjectDisposedException()
        {
            // Arrange.
            Pudge pudge = Setup.Pudge();
            pudge.Despawn();

            // Act and Assert.
            Assert.That(() => pudge.MoveTo(Vector3.one), Throws.TypeOf<ObjectDisposedException>());
        }

        [Test]
        public void Despawn_DespawnAnimationNotEnded_PudgeIsNotDisposed()
        {
            // Arrange.
            Pudge pudge = Setup.Pudge(out var onTick, linearScaleSpeed: PudgeReduceScaleSpeed);
            pudge.Initialize(initialScale: InitialPudgeScale);

            // Act.
            pudge.Despawn();
            onTick.OnNext(new Tick(
                type: TickType.Update,
                deltaTime: InitialPudgeScale / PudgeReduceScaleSpeed / 2f
            ));

            // Assert.
            Assert.That(pudge.Disposed, Is.False);
            Assert.That(pudge.IsDespawning, Is.True);
        }

        [Test]
        public void Despawn_CalledTwiceAndDespawnAnimationEnded_ViewObjectDestroyedOnce()
        {
            // Arrange.
            Pudge pudge = Setup.Pudge(out var onTick, out IPudgeView pudgeView, linearScaleSpeed: PudgeReduceScaleSpeed);
            pudge.Initialize(initialScale: InitialPudgeScale);

            // Act.
            pudge.Despawn();
            pudge.Despawn();
            onTick.OnNext(new Tick(
                type: TickType.Update,
                deltaTime: InitialPudgeScale / PudgeReduceScaleSpeed
            ));
            onTick.OnNext(new Tick(type: TickType.Update, deltaTime: 1f));

            // Assert.
            pudgeView.Received(1).DestroyObject();
        }

        [Test]
        public void MoveTo_WithLinearEasingTypeAndHalfAnimationTime_PudgeIsHalfwayAndTransforming()
        {
            // Arrange.
            Vector3 targetPosition = new(2f, 0f, 0f);

            Pudge pudge = Setup.Pudge(out var onTick, linearMovementSpeed: PudgeLinearMovementSpeed);
            pudge.Initialize();

            float moveDistance = (targetPosition - pudge.CurrentPose.position).magnitude;

            // Act.
            pudge.MoveTo(targetPosition: targetPosition, easingType: Pudge.EasingType.Linear);
            onTick.OnNext(new Tick(
                type: TickType.Update,
                deltaTime: moveDistance / PudgeLinearMovementSpeed / 2f
            ));

            // Assert.
            Assert.That(pudge.CurrentPose.position, Is.EqualTo(targetPosition / 2f).Using(Vector3EqualityComparer.Instance));
        }

        [Test]
        public void MoveTo_WithLinearEasingTypeAndAnimationEnded_PudgeIsNotTransforming()
        {
            // Arrange.
            Vector3 targetPosition = new(2f, 0f, 0f);

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
            Assert.That(pudge.IsTransforming(), Is.False);
        }

        [Test]
        public void RotateTo_WithLinearEasingTypeAcrossZeroYaw_PudgeRotatesByShortestAngle()
        {
            // Arrange.
            Pose initialPose = new(Vector3.zero, Quaternion.Euler(0f, 10f, 0f));
            Quaternion targetRotation = Quaternion.Euler(0f, 350f, 0f);

            Pudge pudge = Setup.Pudge();
            pudge.Initialize(initialPose: initialPose);

            // Act.
            pudge.RotateTo(targetRotation: targetRotation, easingType: Pudge.EasingType.Linear);

            // Assert.
            Assert.That(pudge.RemainingRotationAngle.CurrentValue, Is.EqualTo(-20f).Within(0.001f));
        }

        [Test]
        public void RotateBy_WithLinearEasingTypeCalledTwice_RemainingRotationAngleIsAccumulated()
        {
            // Arrange.
            float rotationAngle = 30f;

            Pudge pudge = Setup.Pudge();
            pudge.Initialize();

            // Act.
            pudge.RotateBy(rotationAngle, Pudge.EasingType.Linear);
            pudge.RotateBy(rotationAngle, Pudge.EasingType.Linear);

            // Assert.
            Assert.That(pudge.RemainingRotationAngle.CurrentValue, Is.EqualTo(rotationAngle * 2f));
        }

        [Test]
        public void ScaleTo_WithLinearEasingTypeAndAnimationEnded_PudgeCurrentScaleIsTargetScale()
        {
            // Arrange.
            float targetScale = 3f;

            Pudge pudge = Setup.Pudge(out var onTick, linearScaleSpeed: PudgeReduceScaleSpeed);
            pudge.Initialize(initialScale: InitialPudgeScale);

            // Act.
            pudge.ScaleTo(targetScale: targetScale, easingType: Pudge.EasingType.Linear);
            onTick.OnNext(new Tick(
                type: TickType.Update,
                deltaTime: (targetScale - InitialPudgeScale) / PudgeReduceScaleSpeed
            ));

            // Assert.
            Assert.That(pudge.CurrentScale, Is.EqualTo(targetScale));
        }
    }
}