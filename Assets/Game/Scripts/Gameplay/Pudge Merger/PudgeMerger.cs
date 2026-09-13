using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace Game.Gameplay
{
    public class PudgeMerger
    {
        private readonly PudgeMergerView _pudgeMergerView;
        private readonly PudgeSpawner _pudgeSpawner;

        private readonly HashSet<Pudge> _mergingPudges = new();
        private bool _enabled;

        private DisposableBag _disposableBag;

        public PudgeMerger(PudgeMergerView pudgeMergerView, PudgeSpawner pudgeSpawner)
        {
            _pudgeMergerView = pudgeMergerView;
            _pudgeSpawner = pudgeSpawner;
        }

        public void Enable()
        {
            if (_enabled)
                return;

            _enabled = true;

            _pudgeSpawner.OnPudgeSpawned
                .Subscribe(MergePudges)
                .AddTo(ref _disposableBag);
        }

        public void Disable()
        {
            if (!_enabled)
                return;

            _enabled = false;

            _disposableBag.Clear();
        }

        private void MergePudges(Pudge firstPudge)
        {
            if (!_enabled || _mergingPudges.Contains(firstPudge))
                return;

            Pudge secondPudge = _pudgeSpawner.SpawnedPudges
                .Where(pudge =>
                    pudge != firstPudge &&
                    !_mergingPudges.Contains(pudge) &&
                    pudge.TargetPosition.CurrentValue == null &&
                    pudge.CurrentState == firstPudge.CurrentState &&
                    pudge.CurrentScale == firstPudge.CurrentScale)
                .OrderBy(pudge => Vector3.Distance(pudge.CurrentPose.position, firstPudge.CurrentPose.position))
                .FirstOrDefault();

            if (secondPudge == null)
                return;

            _mergingPudges.Add(firstPudge);
            _mergingPudges.Add(secondPudge);

            Vector3 middlePoint = (firstPudge.CurrentPose.position + secondPudge.CurrentPose.position) / 2;

            firstPudge.RotateTowards(middlePoint);
            secondPudge.RotateTowards(middlePoint);

            firstPudge.SetTargetPosition(middlePoint);
            secondPudge.SetTargetPosition(middlePoint);

            firstPudge.TargetPosition
                .CombineLatest(secondPudge.TargetPosition,
                    (firstTarget, secondTarget) => !firstTarget.HasValue && !secondTarget.HasValue)
                .Where(bothArrived => bothArrived)
                .Take(1)
                .Subscribe(_ => CompleteMerge(firstPudge, secondPudge))
                .AddTo(ref _disposableBag);
        }

        private void CompleteMerge(Pudge firstPudge, Pudge secondPudge)
        {
            if (!_enabled)
                return;

            Pose pose = firstPudge.CurrentPose;
            Pudge.State state = firstPudge.CurrentState;
            float scale = firstPudge.CurrentScale;

            _pudgeSpawner.DespawnPudge(firstPudge);
            _pudgeSpawner.DespawnPudge(secondPudge);

            _mergingPudges.Remove(firstPudge);
            _mergingPudges.Remove(secondPudge);

            if (scale >= _pudgeMergerView.ScaleToDestroy)
            {
                // Spawn particles.
                return;
            }

            _pudgeSpawner.SpawnPudge(
                pose,
                state,
                scale + _pudgeMergerView.AddScale);
        }
    }
}