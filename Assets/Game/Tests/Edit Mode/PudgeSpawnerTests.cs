using Game.Gameplay;
using NSubstitute;
using NUnit.Framework;
using R3;
using UnityEngine;

namespace Game.Tests.EditMode
{
    public class PudgeSpawnerTests
    {
        public const float PudgeScale = 1f;

        public static readonly Pose PudgePose = new(new Vector3(1f, 2f, 3f), Quaternion.identity);
        public static readonly Pose SpawnMarkerPose = new(new Vector3(4f, 5f, 6f), Quaternion.identity);

        [Test]
        public void SpawnPudge_AnyPudge_PudgeIsAddedToSpawnedPudges()
        {
            // Arrange.
            PudgeSpawner pudgeSpawner = Setup.PudgeSpawner();

            // Act.
            pudgeSpawner.SpawnPudge(PudgePose, Pudge.State.Normal, PudgeScale);

            // Assert.
            Assert.That(pudgeSpawner.SpawnedPudges.Count, Is.EqualTo(1));
        }

        [Test]
        public void SpawnPudge_AnyPudge_OnPudgeSpawnedIsInvoked()
        {
            // Arrange.
            PudgeSpawner pudgeSpawner = Setup.PudgeSpawner();

            Pudge spawnedPudge = null;
            pudgeSpawner.OnPudgeSpawned.Subscribe(pudge => spawnedPudge = pudge);

            // Act.
            pudgeSpawner.SpawnPudge(PudgePose, Pudge.State.Normal, PudgeScale);

            // Assert.
            Assert.That(spawnedPudge, Is.Not.Null);
        }

        [Test]
        public void SpawnPudge_WithHappyState_PudgeObjectIsCreatedForHappyState()
        {
            // Arrange.
            PudgeSpawner pudgeSpawner = Setup.PudgeSpawner(out var _, out var pudgeSpawnerView);

            // Act.
            pudgeSpawner.SpawnPudge(PudgePose, Pudge.State.Happy, PudgeScale);

            // Assert.
            pudgeSpawnerView.Received(1).CreatePudgeObject(Pudge.State.Happy);
        }

        [Test]
        public void DespawnPudge_SpawnedPudge_PudgeIsRemovedFromSpawnedPudges()
        {
            // Arrange.
            PudgeSpawner pudgeSpawner = Setup.PudgeSpawner();

            Pudge spawnedPudge = null;
            pudgeSpawner.OnPudgeSpawned.Subscribe(pudge => spawnedPudge = pudge);
            pudgeSpawner.SpawnPudge(PudgePose, Pudge.State.Normal, PudgeScale);

            // Act.
            pudgeSpawner.DespawnPudge(spawnedPudge);

            // Assert.
            Assert.That(pudgeSpawner.SpawnedPudges, Is.Empty);
        }

        [Test]
        public void DespawnPudge_SpawnedPudge_PudgeIsDespawning()
        {
            // Arrange.
            PudgeSpawner pudgeSpawner = Setup.PudgeSpawner();

            Pudge spawnedPudge = null;
            pudgeSpawner.OnPudgeSpawned.Subscribe(pudge => spawnedPudge = pudge);
            pudgeSpawner.SpawnPudge(PudgePose, Pudge.State.Normal, PudgeScale);

            // Act.
            pudgeSpawner.DespawnPudge(spawnedPudge);

            // Assert.
            Assert.That(spawnedPudge.IsDespawning, Is.True);
        }

        [Test]
        public void SpawnMarkerReleased_InitialStateIsSet_PudgeIsSpawned()
        {
            // Arrange.
            PudgeSpawner pudgeSpawner = Setup.PudgeSpawner(out var onSpawnMarkerReleased);
            pudgeSpawner.SetInitialPudgeState(Pudge.State.Normal);
            pudgeSpawner.Enable();

            // Act.
            onSpawnMarkerReleased.OnNext(SpawnMarkerPose);

            // Assert.
            Assert.That(pudgeSpawner.SpawnedPudges.Count, Is.EqualTo(1));
        }

        [Test]
        public void SpawnMarkerReleased_InitialStateIsSet_PudgeIsSpawnedAtSpawnMarkerPosition()
        {
            // Arrange.
            PudgeSpawner pudgeSpawner = Setup.PudgeSpawner(out var onSpawnMarkerReleased);
            pudgeSpawner.SetInitialPudgeState(Pudge.State.Normal);
            pudgeSpawner.Enable();

            Pudge spawnedPudge = null;
            pudgeSpawner.OnPudgeSpawned.Subscribe(pudge => spawnedPudge = pudge);

            // Act.
            onSpawnMarkerReleased.OnNext(SpawnMarkerPose);

            // Assert.
            Assert.That(spawnedPudge.CurrentPose.position, Is.EqualTo(SpawnMarkerPose.position));
        }

        [Test]
        public void SpawnMarkerReleased_InitialStateIsNone_PudgeIsNotSpawned()
        {
            // Arrange.
            PudgeSpawner pudgeSpawner = Setup.PudgeSpawner(out var onSpawnMarkerReleased);
            pudgeSpawner.SetInitialPudgeState(Pudge.State.None);
            pudgeSpawner.Enable();

            // Act.
            onSpawnMarkerReleased.OnNext(SpawnMarkerPose);

            // Assert.
            Assert.That(pudgeSpawner.SpawnedPudges, Is.Empty);
        }

        [Test]
        public void SpawnMarkerReleased_PudgeSpawnerIsDisabled_PudgeIsNotSpawned()
        {
            // Arrange.
            PudgeSpawner pudgeSpawner = Setup.PudgeSpawner(out var onSpawnMarkerReleased);
            pudgeSpawner.SetInitialPudgeState(Pudge.State.Normal);
            pudgeSpawner.Enable();
            pudgeSpawner.Disable();

            // Act.
            onSpawnMarkerReleased.OnNext(SpawnMarkerPose);

            // Assert.
            Assert.That(pudgeSpawner.SpawnedPudges, Is.Empty);
        }
    }
}
