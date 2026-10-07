using Game.Core.Input;
using Game.Gameplay;
using NSubstitute;
using NUnit.Framework;
using R3;
using UnityEngine;

namespace Game.Tests.EditMode
{
    public class SpawnMarkerCreatorTests
    {
        public static readonly Vector2 PressScreenPosition = new(100f, 100f);
        public static readonly Vector2 DragScreenPosition = new(200f, 200f);

        public static readonly Pose PressFloorPose = new(new Vector3(1f, 0f, 1f), Quaternion.identity);
        public static readonly Pose DragFloorPose = new(new Vector3(2f, 0f, 2f), Quaternion.identity);

        public static readonly InputContext PressStarted = new()
        {
            ActionType = ActionType.Press,
            ActionStatus = ActionStatus.Started,
            ScreenPosition = PressScreenPosition,
            IsOverUI = false
        };

        public static readonly InputContext PressStartedOverUI = new()
        {
            ActionType = ActionType.Press,
            ActionStatus = ActionStatus.Started,
            ScreenPosition = PressScreenPosition,
            IsOverUI = true
        };

        public static readonly InputContext DragPerformed = new()
        {
            ActionType = ActionType.Drag,
            ActionStatus = ActionStatus.Performed,
            ScreenPosition = DragScreenPosition,
            IsOverUI = false
        };

        public static readonly InputContext DragPerformedOverUI = new()
        {
            ActionType = ActionType.Drag,
            ActionStatus = ActionStatus.Performed,
            ScreenPosition = DragScreenPosition,
            IsOverUI = true
        };

        public static readonly InputContext PressCanceled = new()
        {
            ActionType = ActionType.Press,
            ActionStatus = ActionStatus.Canceled
        };

        [Test]
        public void PressStarted_RaycastHitsFloor_SpawnMarkerIsCreated()
        {
            // Arrange.
            SpawnMarkerCreator spawnMarkerCreator = Setup.SpawnMarkerCreator(out var onInputActionPerformed, out var raycastService);
            spawnMarkerCreator.Enable();

            raycastService
                .RaycastOnFloor(Arg.Is(PressScreenPosition), out Arg.Any<Pose>())
                .Returns(call => { call[1] = PressFloorPose; return true; });

            // Act.
            onInputActionPerformed.OnNext(PressStarted);

            // Assert.
            Assert.That(spawnMarkerCreator.CurrentSpawnMarker, Is.Not.Null);
        }

        [Test]
        public void PressStarted_RaycastMissesFloor_SpawnMarkerIsNotCreated()
        {
            // Arrange.
            SpawnMarkerCreator spawnMarkerCreator = Setup.SpawnMarkerCreator(out var onInputActionPerformed, out var _);
            spawnMarkerCreator.Enable();

            // Act.
            onInputActionPerformed.OnNext(PressStarted);

            // Assert.
            Assert.That(spawnMarkerCreator.CurrentSpawnMarker, Is.Null);
        }

        [Test]
        public void PressStarted_OverUI_SpawnMarkerIsNotCreated()
        {
            // Arrange.
            SpawnMarkerCreator spawnMarkerCreator = Setup.SpawnMarkerCreator(out var onInputActionPerformed, out var raycastService);
            spawnMarkerCreator.Enable();

            raycastService
                .RaycastOnFloor(Arg.Is(PressScreenPosition), out Arg.Any<Pose>())
                .Returns(call => { call[1] = PressFloorPose; return true; });

            // Act.
            onInputActionPerformed.OnNext(PressStartedOverUI);

            // Assert.
            Assert.That(spawnMarkerCreator.CurrentSpawnMarker, Is.Null);
        }

        [Test]
        public void DragPerformed_RaycastHitsFloor_SpawnMarkerIsMovedToDragPosition()
        {
            // Arrange.
            SpawnMarkerCreator spawnMarkerCreator = Setup.SpawnMarkerCreator(out var onInputActionPerformed, out var raycastService);
            spawnMarkerCreator.Enable();

            raycastService
                .RaycastOnFloor(Arg.Is(PressScreenPosition), out Arg.Any<Pose>())
                .Returns(call => { call[1] = PressFloorPose; return true; });
            raycastService
                .RaycastOnFloor(Arg.Is(DragScreenPosition), out Arg.Any<Pose>())
                .Returns(call => { call[1] = DragFloorPose; return true; });

            onInputActionPerformed.OnNext(PressStarted);

            // Act.
            onInputActionPerformed.OnNext(DragPerformed);

            // Assert.
            Assert.That(spawnMarkerCreator.CurrentSpawnMarker.Pose.position, Is.EqualTo(DragFloorPose.position));
        }

        [Test]
        public void DragPerformed_OverUI_SpawnMarkerIsDeleted()
        {
            // Arrange.
            SpawnMarkerCreator spawnMarkerCreator = Setup.SpawnMarkerCreator(out var onInputActionPerformed, out var raycastService);
            spawnMarkerCreator.Enable();

            raycastService
                .RaycastOnFloor(Arg.Is(PressScreenPosition), out Arg.Any<Pose>())
                .Returns(call => { call[1] = PressFloorPose; return true; });

            onInputActionPerformed.OnNext(PressStarted);

            // Act.
            onInputActionPerformed.OnNext(DragPerformedOverUI);

            // Assert.
            Assert.That(spawnMarkerCreator.CurrentSpawnMarker, Is.Null);
        }

        [Test]
        public void PressCanceled_SpawnMarkerExists_SpawnMarkerIsReleasedWithItsPose()
        {
            // Arrange.
            SpawnMarkerCreator spawnMarkerCreator = Setup.SpawnMarkerCreator(out var onInputActionPerformed, out var raycastService);
            spawnMarkerCreator.Enable();

            raycastService
                .RaycastOnFloor(Arg.Is(PressScreenPosition), out Arg.Any<Pose>())
                .Returns(call => { call[1] = PressFloorPose; return true; });

            Pose? releasedPose = null;
            spawnMarkerCreator.OnSpawnMarkerReleased.Subscribe(pose => releasedPose = pose);

            onInputActionPerformed.OnNext(PressStarted);

            // Act.
            onInputActionPerformed.OnNext(PressCanceled);

            // Assert.
            Assert.That(releasedPose?.position, Is.EqualTo(PressFloorPose.position));
        }

        [Test]
        public void PressCanceled_SpawnMarkerExists_SpawnMarkerIsDeletedBeforeRelease()
        {
            // Arrange.
            SpawnMarkerCreator spawnMarkerCreator = Setup.SpawnMarkerCreator(out var onInputActionPerformed, out var raycastService);
            spawnMarkerCreator.Enable();

            raycastService
                .RaycastOnFloor(Arg.Is(PressScreenPosition), out Arg.Any<Pose>())
                .Returns(call => { call[1] = PressFloorPose; return true; });

            bool spawnMarkerExistsOnRelease = true;
            spawnMarkerCreator.OnSpawnMarkerReleased
                .Subscribe(_ => spawnMarkerExistsOnRelease = spawnMarkerCreator.CurrentSpawnMarker != null);

            onInputActionPerformed.OnNext(PressStarted);

            // Act.
            onInputActionPerformed.OnNext(PressCanceled);

            // Assert.
            Assert.That(spawnMarkerExistsOnRelease, Is.False);
        }

        [Test]
        public void PressCanceled_NoSpawnMarker_SpawnMarkerIsNotReleased()
        {
            // Arrange.
            SpawnMarkerCreator spawnMarkerCreator = Setup.SpawnMarkerCreator(out var onInputActionPerformed, out var _);
            spawnMarkerCreator.Enable();

            bool released = false;
            spawnMarkerCreator.OnSpawnMarkerReleased.Subscribe(_ => released = true);

            // Act.
            onInputActionPerformed.OnNext(PressCanceled);

            // Assert.
            Assert.That(released, Is.False);
        }

        [Test]
        public void PressCanceled_AfterDragOverUI_SpawnMarkerIsNotReleased()
        {
            // Arrange.
            SpawnMarkerCreator spawnMarkerCreator = Setup.SpawnMarkerCreator(out var onInputActionPerformed, out var raycastService);
            spawnMarkerCreator.Enable();

            raycastService
                .RaycastOnFloor(Arg.Is(PressScreenPosition), out Arg.Any<Pose>())
                .Returns(call => { call[1] = PressFloorPose; return true; });

            bool released = false;
            spawnMarkerCreator.OnSpawnMarkerReleased.Subscribe(_ => released = true);

            onInputActionPerformed.OnNext(PressStarted);
            onInputActionPerformed.OnNext(DragPerformedOverUI);

            // Act.
            onInputActionPerformed.OnNext(PressCanceled);

            // Assert.
            Assert.That(released, Is.False);
        }

        [Test]
        public void Disable_SpawnMarkerExists_SpawnMarkerIsDeleted()
        {
            // Arrange.
            SpawnMarkerCreator spawnMarkerCreator = Setup.SpawnMarkerCreator(out var onInputActionPerformed, out var raycastService);
            spawnMarkerCreator.Enable();

            raycastService
                .RaycastOnFloor(Arg.Is(PressScreenPosition), out Arg.Any<Pose>())
                .Returns(call => { call[1] = PressFloorPose; return true; });

            onInputActionPerformed.OnNext(PressStarted);

            // Act.
            spawnMarkerCreator.Disable();

            // Assert.
            Assert.That(spawnMarkerCreator.CurrentSpawnMarker, Is.Null);
        }
    }
}
