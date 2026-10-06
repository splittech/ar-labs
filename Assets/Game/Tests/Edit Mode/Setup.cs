using System.Collections.Generic;
using Game.Core;
using Game.Core.AR;
using Game.Core.Input;
using Game.Gameplay;
using NSubstitute;
using R3;
using UnityEngine;

namespace Game.Tests
{
    public static class Setup
    {
        public static Pudge Pudge(
            float linearMovementSpeed = 1f,
            float linearRotationSpeed = 1f,
            float linearScaleSpeed = 1f)
        {
            return Pudge(out _, out _, linearMovementSpeed, linearRotationSpeed, linearScaleSpeed);
        }

        public static Pudge Pudge(
            out Subject<Tick> onTick,
            float linearMovementSpeed = 1f,
            float linearRotationSpeed = 1f,
            float linearScaleSpeed = 1f)
        {
            return Pudge(out onTick, out _, linearMovementSpeed, linearRotationSpeed, linearScaleSpeed);
        }

        public static Pudge Pudge(
            out Subject<Tick> onTick,
            out IPudgeView pudgeView,
            float linearMovementSpeed = 1f,
            float linearRotationSpeed = 1f,
            float linearScaleSpeed = 1f)
        {
            onTick = new Subject<Tick>();

            return Pudge(onTick, out pudgeView, linearMovementSpeed, linearRotationSpeed, linearScaleSpeed);
        }

        public static Pudge Pudge(
            Subject<Tick> onTick,
            float linearMovementSpeed = 1f,
            float linearRotationSpeed = 1f,
            float linearScaleSpeed = 1f)
        {
            return Pudge(onTick, out _, linearMovementSpeed, linearRotationSpeed, linearScaleSpeed);
        }

        public static Pudge Pudge(
            Subject<Tick> onTick,
            out IPudgeView pudgeView,
            float linearMovementSpeed = 1f,
            float linearRotationSpeed = 1f,
            float linearScaleSpeed = 1f)
        {
            pudgeView = Substitute.For<IPudgeView>();
            pudgeView.LinearMovementSpeed.Returns(linearMovementSpeed);
            pudgeView.LinearRotationSpeed.Returns(linearRotationSpeed);
            pudgeView.LinearScaleSpeed.Returns(linearScaleSpeed);

            var tickService = Substitute.For<ITickService>();
            tickService.OnTick.Returns(onTick);

            Pudge pudge = new(pudgeView, tickService);
            return pudge;
        }

        public static PudgeMerger PudgeMerger(
            out Subject<Tick> onTick,
            out Subject<Pudge> onPudgeSpawned,
            out IPudgeSpawner pudgeSpawner,
            float addScale = 1f,
            float scaleToDestroy = 3f)
        {
            return PudgeMerger(out onTick, out onPudgeSpawned, out pudgeSpawner, out _, addScale, scaleToDestroy);
        }

        public static PudgeMerger PudgeMerger(
            out Subject<Tick> onTick,
            out Subject<Pudge> onPudgeSpawned,
            out IPudgeSpawner pudgeSpawner,
            out IPudgeMergerView pudgeMergerView,
            float addScale = 1f,
            float scaleToDestroy = 3f)
        {
            return PudgeMerger(out onTick, out onPudgeSpawned, out pudgeSpawner, out pudgeMergerView, out _, addScale, scaleToDestroy);
        }

        public static PudgeMerger PudgeMerger(
            out Subject<Tick> onTick,
            out Subject<Pudge> onPudgeSpawned,
            out IPudgeSpawner pudgeSpawner,
            out IPudgeMergerView pudgeMergerView,
            out ReactiveProperty<Pudge> previousSelectedPudge,
            float addScale = 1f,
            float scaleToDestroy = 3f)
        {
            onTick = new Subject<Tick>();
            onPudgeSpawned = new Subject<Pudge>();

            pudgeSpawner = Substitute.For<IPudgeSpawner>();
            pudgeSpawner.SpawnedPudges.Returns(new HashSet<Pudge>());
            pudgeSpawner.OnPudgeSpawned.Returns(onPudgeSpawned);

            var pudgeEditor = Substitute.For<IPudgeEditor>();
            previousSelectedPudge = new ReactiveProperty<Pudge>();
            pudgeEditor.PreviousSelectedPudge.Returns(previousSelectedPudge);

            pudgeMergerView = Substitute.For<IPudgeMergerView>();
            pudgeMergerView.AddScale.Returns(addScale);
            pudgeMergerView.ScaleToDestroy.Returns(scaleToDestroy);

            PudgeMerger pudgeMerger = new(pudgeMergerView, pudgeSpawner, pudgeEditor);
            return pudgeMerger;
        }

        public static PudgeEditor PudgeEditor(
            out Subject<InputContext> onInputActionPerformed,
            out IRaycastService raycastService,
            float scaleDelta = 0.5f,
            float rotationDelta = 90f)
        {
            onInputActionPerformed = new Subject<InputContext>();

            var inputService = Substitute.For<IInputService>();
            inputService.OnInputActionPerformed.Returns(onInputActionPerformed);

            raycastService = Substitute.For<IRaycastService>();

            var pudgeEditorView = Substitute.For<IPudgeEditorView>();
            pudgeEditorView.ScaleDelta.Returns(scaleDelta);
            pudgeEditorView.RotationDelta.Returns(rotationDelta);

            PudgeEditor pudgeEditor = new(inputService, raycastService, pudgeEditorView);
            return pudgeEditor;
        }

        public static PudgeSpawner PudgeSpawner(float initialScale = 1f)
        {
            return PudgeSpawner(out _, out _, initialScale);
        }

        public static PudgeSpawner PudgeSpawner(
            out Subject<Pose> onSpawnMarkerReleased,
            float initialScale = 1f)
        {
            return PudgeSpawner(out onSpawnMarkerReleased, out _, initialScale);
        }

        public static PudgeSpawner PudgeSpawner(
            out Subject<Pose> onSpawnMarkerReleased,
            out IPudgeSpawnerView pudgeSpawnerView,
            float initialScale = 1f)
        {
            onSpawnMarkerReleased = new Subject<Pose>();

            var spawnMarkerCreator = Substitute.For<ISpawnMarkerCreator>();
            spawnMarkerCreator.OnSpawnMarkerReleased.Returns(onSpawnMarkerReleased);

            pudgeSpawnerView = Substitute.For<IPudgeSpawnerView>();
            pudgeSpawnerView.InitialScale.Returns(initialScale);
            pudgeSpawnerView.CreatePudgeObject(Arg.Any<Pudge.State>()).Returns(_ => Substitute.For<IPudgeView>());

            var tickService = Substitute.For<ITickService>();
            tickService.OnTick.Returns(new Subject<Tick>());

            PudgeSpawner pudgeSpawner = new(pudgeSpawnerView, spawnMarkerCreator, tickService);
            return pudgeSpawner;
        }

        public static Timer Timer(out Subject<Tick> onTick)
        {
            onTick = new Subject<Tick>();

            var tickService = Substitute.For<ITickService>();
            tickService.OnTick.Returns(onTick);

            Timer timer = new(tickService);
            return timer;
        }

        public static CrossDetector CrossDetector(
            float maxDiagonalDeltaAngle = 30f,
            float maxDeltaTimeBetweenTwoSwipes = 1f)
        {
            return CrossDetector(out _, maxDiagonalDeltaAngle, maxDeltaTimeBetweenTwoSwipes);
        }

        public static CrossDetector CrossDetector(
            out ReactiveProperty<bool> timerElapsed,
            float maxDiagonalDeltaAngle = 30f,
            float maxDeltaTimeBetweenTwoSwipes = 1f)
        {
            timerElapsed = new ReactiveProperty<bool>(false);

            var crossDetectorView = Substitute.For<ICrossDetectorView>();
            crossDetectorView.MaxDiagonalDeltaAngle.Returns(maxDiagonalDeltaAngle);
            crossDetectorView.MaxDeltaTimeBwetweenTwoSwipes.Returns(maxDeltaTimeBetweenTwoSwipes);

            var timer = Substitute.For<ITimer>();
            timer.Elapsed.Returns(timerElapsed);

            var timerService = Substitute.For<ITimerService>();
            timerService.CreateTimer().Returns(timer);

            CrossDetector crossDetector = new(crossDetectorView, timerService);
            return crossDetector;
        }
    }
}