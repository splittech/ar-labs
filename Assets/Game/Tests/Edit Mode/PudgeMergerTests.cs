using Game.Core;
using Game.Gameplay;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode
{
    public class PudgeMergerTests
    {
        public const float PudgeScale = 1f;
        public const float AddScale = 1f;
        public const float ScaleToDestroy = 3f;

        public static readonly Pose FirstPudgePose = new(new Vector3(0f, 0f, 0f), Quaternion.identity);
        public static readonly Pose SecondPudgePose = new(new Vector3(2f, 0f, 0f), Quaternion.identity);

        // Пуджи идут навстречу с LinearMovementSpeed = 1 из Setup, каждому нужно пройти половину расстояния.
        public static readonly float MergeTime = Vector3.Distance(FirstPudgePose.position, SecondPudgePose.position) / 2f;

        [Test]
        public void OnPudgeSpawned_PudgeWithSameStateAndScaleExists_SpawnedPudgeIsTransforming()
        {
            // Arrange.
            PudgeMerger pudgeMerger = Setup.PudgeMerger(out var onTick, out var onPudgeSpawned, out var pudgeSpawner);
            pudgeMerger.Enable();

            Pudge secondPudge = Setup.Pudge(onTick);
            secondPudge.Initialize(initialPose: SecondPudgePose);
            pudgeSpawner.SpawnedPudges.Add(secondPudge);

            Pudge spawnedPudge = Setup.Pudge(onTick);
            spawnedPudge.Initialize(initialPose: FirstPudgePose);
            pudgeSpawner.SpawnedPudges.Add(spawnedPudge);

            // Act.
            onPudgeSpawned.OnNext(spawnedPudge);

            // Assert.
            Assert.That(spawnedPudge.IsTransforming(), Is.True);
        }

        [Test]
        public void OnPudgeSpawned_PudgeWithDifferentStateExists_SpawnedPudgeIsNotTransforming()
        {
            // Arrange.
            PudgeMerger pudgeMerger = Setup.PudgeMerger(out var onTick, out var onPudgeSpawned, out var pudgeSpawner);
            pudgeMerger.Enable();

            Pudge secondPudge = Setup.Pudge(onTick);
            secondPudge.Initialize(initialPose: SecondPudgePose, initialState: Pudge.State.Sad);
            pudgeSpawner.SpawnedPudges.Add(secondPudge);

            Pudge spawnedPudge = Setup.Pudge(onTick);
            spawnedPudge.Initialize(initialPose: FirstPudgePose, initialState: Pudge.State.Happy);
            pudgeSpawner.SpawnedPudges.Add(spawnedPudge);

            // Act.
            onPudgeSpawned.OnNext(spawnedPudge);

            // Assert.
            Assert.That(spawnedPudge.IsTransforming(), Is.False);
        }

        [Test]
        public void OnPudgeSpawned_PudgeWithDifferentScaleExists_SpawnedPudgeIsNotTransforming()
        {
            // Arrange.
            PudgeMerger pudgeMerger = Setup.PudgeMerger(out var onTick, out var onPudgeSpawned, out var pudgeSpawner);
            pudgeMerger.Enable();

            Pudge secondPudge = Setup.Pudge(onTick);
            secondPudge.Initialize(initialPose: SecondPudgePose, initialScale: PudgeScale + AddScale);
            pudgeSpawner.SpawnedPudges.Add(secondPudge);

            Pudge spawnedPudge = Setup.Pudge(onTick);
            spawnedPudge.Initialize(initialPose: FirstPudgePose, initialScale: PudgeScale);
            pudgeSpawner.SpawnedPudges.Add(spawnedPudge);

            // Act.
            onPudgeSpawned.OnNext(spawnedPudge);

            // Assert.
            Assert.That(spawnedPudge.IsTransforming(), Is.False);
        }

        [Test]
        public void OnPudgeSpawned_SeveralSuitablePudgesExist_NearestPudgeIsTransforming()
        {
            // Arrange.
            Pose farthestPudgePose = new(SecondPudgePose.position * 5f, Quaternion.identity);

            PudgeMerger pudgeMerger = Setup.PudgeMerger(out var onTick, out var onPudgeSpawned, out var pudgeSpawner);
            pudgeMerger.Enable();

            Pudge farthestPudge = Setup.Pudge(onTick);
            farthestPudge.Initialize(initialPose: farthestPudgePose);
            pudgeSpawner.SpawnedPudges.Add(farthestPudge);

            Pudge nearestPudge = Setup.Pudge(onTick);
            nearestPudge.Initialize(initialPose: SecondPudgePose);
            pudgeSpawner.SpawnedPudges.Add(nearestPudge);

            Pudge spawnedPudge = Setup.Pudge(onTick);
            spawnedPudge.Initialize(initialPose: FirstPudgePose);
            pudgeSpawner.SpawnedPudges.Add(spawnedPudge);

            // Act.
            onPudgeSpawned.OnNext(spawnedPudge);

            // Assert.
            Assert.That(nearestPudge.IsTransforming(), Is.True);
        }

        [Test]
        public void OnPudgeSpawned_OnlyMergingPudgesExist_SpawnedPudgeIsNotTransforming()
        {
            // Arrange.
            Pose thirdPudgePose = new(-SecondPudgePose.position, Quaternion.identity);

            PudgeMerger pudgeMerger = Setup.PudgeMerger(out var onTick, out var onPudgeSpawned, out var pudgeSpawner);
            pudgeMerger.Enable();

            Pudge secondPudge = Setup.Pudge(onTick);
            secondPudge.Initialize(initialPose: SecondPudgePose);
            pudgeSpawner.SpawnedPudges.Add(secondPudge);

            Pudge firstPudge = Setup.Pudge(onTick);
            firstPudge.Initialize(initialPose: FirstPudgePose);
            pudgeSpawner.SpawnedPudges.Add(firstPudge);

            onPudgeSpawned.OnNext(firstPudge);

            Pudge spawnedPudge = Setup.Pudge(onTick);
            spawnedPudge.Initialize(initialPose: thirdPudgePose);
            pudgeSpawner.SpawnedPudges.Add(spawnedPudge);

            // Act.
            onPudgeSpawned.OnNext(spawnedPudge);

            // Assert.
            Assert.That(spawnedPudge.IsTransforming(), Is.False);
        }

        [Test]
        public void OnPudgeSpawned_MergeEnded_BothPudgesAreDespawned()
        {
            // Arrange.
            PudgeMerger pudgeMerger = Setup.PudgeMerger(out var onTick, out var onPudgeSpawned, out var pudgeSpawner);
            pudgeMerger.Enable();

            Pudge secondPudge = Setup.Pudge(onTick);
            secondPudge.Initialize(initialPose: SecondPudgePose);
            pudgeSpawner.SpawnedPudges.Add(secondPudge);

            Pudge spawnedPudge = Setup.Pudge(onTick);
            spawnedPudge.Initialize(initialPose: FirstPudgePose);
            pudgeSpawner.SpawnedPudges.Add(spawnedPudge);

            // Act.
            onPudgeSpawned.OnNext(spawnedPudge);
            onTick.OnNext(new Tick(type: TickType.Update, deltaTime: MergeTime));

            // Assert.
            pudgeSpawner.Received(2).DespawnPudge(Arg.Any<Pudge>());
        }

        [Test]
        public void OnPudgeSpawned_MergeEndedAndScaleBelowScaleToDestroy_BiggerPudgeIsSpawned()
        {
            // Arrange.
            PudgeMerger pudgeMerger = Setup.PudgeMerger(
                out var onTick, out var onPudgeSpawned, out var pudgeSpawner,
                addScale: AddScale, scaleToDestroy: ScaleToDestroy);
            pudgeMerger.Enable();

            Pudge secondPudge = Setup.Pudge(onTick);
            secondPudge.Initialize(initialPose: SecondPudgePose, initialScale: PudgeScale);
            pudgeSpawner.SpawnedPudges.Add(secondPudge);

            Pudge spawnedPudge = Setup.Pudge(onTick);
            spawnedPudge.Initialize(initialPose: FirstPudgePose, initialScale: PudgeScale);
            pudgeSpawner.SpawnedPudges.Add(spawnedPudge);

            // Act.
            onPudgeSpawned.OnNext(spawnedPudge);
            onTick.OnNext(new Tick(type: TickType.Update, deltaTime: MergeTime));

            // Assert.
            pudgeSpawner.Received(1).SpawnPudge(Arg.Any<Pose>(), Pudge.State.Normal, PudgeScale + AddScale);
        }

        [Test]
        public void OnPudgeSpawned_MergeEndedAndScaleReachedScaleToDestroy_FinalEffectIsCreated()
        {
            // Arrange.
            PudgeMerger pudgeMerger = Setup.PudgeMerger(
                out var onTick, out var onPudgeSpawned, out var pudgeSpawner, out var pudgeMergerView,
                addScale: AddScale, scaleToDestroy: ScaleToDestroy);
            pudgeMerger.Enable();

            Pudge secondPudge = Setup.Pudge(onTick);
            secondPudge.Initialize(initialPose: SecondPudgePose, initialScale: ScaleToDestroy);
            pudgeSpawner.SpawnedPudges.Add(secondPudge);

            Pudge spawnedPudge = Setup.Pudge(onTick);
            spawnedPudge.Initialize(initialPose: FirstPudgePose, initialScale: ScaleToDestroy);
            pudgeSpawner.SpawnedPudges.Add(spawnedPudge);

            // Act.
            onPudgeSpawned.OnNext(spawnedPudge);
            onTick.OnNext(new Tick(type: TickType.Update, deltaTime: MergeTime));

            // Assert.
            pudgeMergerView.Received(1).CreateFinalEffect(Arg.Any<Vector3>());
        }

        [Test]
        public void OnPudgeSpawned_PudgeMergerIsDisabled_SpawnedPudgeIsNotTransforming()
        {
            // Arrange.
            PudgeMerger pudgeMerger = Setup.PudgeMerger(out var onTick, out var onPudgeSpawned, out var pudgeSpawner);
            pudgeMerger.Enable();
            pudgeMerger.Disable();

            Pudge secondPudge = Setup.Pudge(onTick);
            secondPudge.Initialize(initialPose: SecondPudgePose);
            pudgeSpawner.SpawnedPudges.Add(secondPudge);

            Pudge spawnedPudge = Setup.Pudge(onTick);
            spawnedPudge.Initialize(initialPose: FirstPudgePose);
            pudgeSpawner.SpawnedPudges.Add(spawnedPudge);

            // Act.
            onPudgeSpawned.OnNext(spawnedPudge);

            // Assert.
            Assert.That(spawnedPudge.IsTransforming(), Is.False);
        }

        [Test]
        public void Disable_MergeNotEnded_PudgesAreNotDespawned()
        {
            // Arrange.
            PudgeMerger pudgeMerger = Setup.PudgeMerger(out var onTick, out var onPudgeSpawned, out var pudgeSpawner);
            pudgeMerger.Enable();

            Pudge secondPudge = Setup.Pudge(onTick);
            secondPudge.Initialize(initialPose: SecondPudgePose);
            pudgeSpawner.SpawnedPudges.Add(secondPudge);

            Pudge spawnedPudge = Setup.Pudge(onTick);
            spawnedPudge.Initialize(initialPose: FirstPudgePose);
            pudgeSpawner.SpawnedPudges.Add(spawnedPudge);

            onPudgeSpawned.OnNext(spawnedPudge);

            // Act.
            pudgeMerger.Disable();
            onTick.OnNext(new Tick(type: TickType.Update, deltaTime: MergeTime));

            // Assert.
            pudgeSpawner.DidNotReceive().DespawnPudge(Arg.Any<Pudge>());
        }

        [Test]
        public void PreviousSelectedPudgeChanged_PudgeWithSameStateAndScaleExists_DeselectedPudgeIsTransforming()
        {
            // Arrange.
            PudgeMerger pudgeMerger = Setup.PudgeMerger(
                out var onTick, out var _, out var pudgeSpawner, out var _, out var previousSelectedPudge);
            pudgeMerger.Enable();

            Pudge secondPudge = Setup.Pudge(onTick);
            secondPudge.Initialize(initialPose: SecondPudgePose);
            pudgeSpawner.SpawnedPudges.Add(secondPudge);

            Pudge deselectedPudge = Setup.Pudge(onTick);
            deselectedPudge.Initialize(initialPose: FirstPudgePose);
            pudgeSpawner.SpawnedPudges.Add(deselectedPudge);

            // Act.
            previousSelectedPudge.Value = deselectedPudge;

            // Assert.
            Assert.That(deselectedPudge.IsTransforming(), Is.True);
        }

        [Test]
        public void PreviousSelectedPudgeChanged_DeselectedPudgeIsTransforming_OtherPudgeIsNotTransforming()
        {
            // Arrange.
            PudgeMerger pudgeMerger = Setup.PudgeMerger(
                out var onTick, out var _, out var pudgeSpawner, out var _, out var previousSelectedPudge);
            pudgeMerger.Enable();

            Pudge otherPudge = Setup.Pudge(onTick);
            otherPudge.Initialize(initialPose: SecondPudgePose);
            pudgeSpawner.SpawnedPudges.Add(otherPudge);

            Pudge deselectedPudge = Setup.Pudge(onTick);
            deselectedPudge.Initialize(initialPose: FirstPudgePose);
            pudgeSpawner.SpawnedPudges.Add(deselectedPudge);

            deselectedPudge.RotateBy(90f, Pudge.EasingType.Linear);

            // Act.
            previousSelectedPudge.Value = deselectedPudge;

            // Assert.
            Assert.That(otherPudge.IsTransforming(), Is.False);
        }

        [Test]
        public void PreviousSelectedPudgeChanged_MergeEnded_BothPudgesAreDespawned()
        {
            // Arrange.
            PudgeMerger pudgeMerger = Setup.PudgeMerger(
                out var onTick, out var _, out var pudgeSpawner, out var _, out var previousSelectedPudge);
            pudgeMerger.Enable();

            Pudge secondPudge = Setup.Pudge(onTick);
            secondPudge.Initialize(initialPose: SecondPudgePose);
            pudgeSpawner.SpawnedPudges.Add(secondPudge);

            Pudge deselectedPudge = Setup.Pudge(onTick);
            deselectedPudge.Initialize(initialPose: FirstPudgePose);
            pudgeSpawner.SpawnedPudges.Add(deselectedPudge);

            // Act.
            previousSelectedPudge.Value = deselectedPudge;
            onTick.OnNext(new Tick(type: TickType.Update, deltaTime: MergeTime));

            // Assert.
            pudgeSpawner.Received(2).DespawnPudge(Arg.Any<Pudge>());
        }

        [Test]
        public void PreviousSelectedPudgeChanged_PudgeMergerIsDisabled_DeselectedPudgeIsNotTransforming()
        {
            // Arrange.
            PudgeMerger pudgeMerger = Setup.PudgeMerger(
                out var onTick, out var _, out var pudgeSpawner, out var _, out var previousSelectedPudge);
            pudgeMerger.Enable();
            pudgeMerger.Disable();

            Pudge secondPudge = Setup.Pudge(onTick);
            secondPudge.Initialize(initialPose: SecondPudgePose);
            pudgeSpawner.SpawnedPudges.Add(secondPudge);

            Pudge deselectedPudge = Setup.Pudge(onTick);
            deselectedPudge.Initialize(initialPose: FirstPudgePose);
            pudgeSpawner.SpawnedPudges.Add(deselectedPudge);

            // Act.
            previousSelectedPudge.Value = deselectedPudge;

            // Assert.
            Assert.That(deselectedPudge.IsTransforming(), Is.False);
        }
    }
}
