using System.Collections.Generic;
using System.Linq;
using Game.Core;
using R3;
using UnityEngine;

namespace Game.Gameplay
{
    public class PudgeSpawner : IPudgeSpawner
    {
        private readonly IPudgeSpawnerView _pudgeSpawnerView;
        private readonly ISpawnMarkerCreator _spawnMarkerCreator;
        private readonly ITickService _tickService;

        private HashSet<Pudge> _spawnedPudges = new();
        private Pudge.State _inititalPudgeState;
        private bool _enabled;

        private DisposableBag _disposableBag;

        public HashSet<Pudge> SpawnedPudges => _spawnedPudges;

        private Subject<Pudge> _onPudgeSpawned = new();
        public Observable<Pudge> OnPudgeSpawned => _onPudgeSpawned;

        public PudgeSpawner(
            IPudgeSpawnerView pudgeSpawnerView,
            ISpawnMarkerCreator spawnMarkerCreator,
            ITickService tickService)
        {
            _pudgeSpawnerView = pudgeSpawnerView;
            _spawnMarkerCreator = spawnMarkerCreator;
            _tickService = tickService;
        }

        public void Enable()
        {
            if (_enabled)
                return;

            _enabled = true;

            _spawnMarkerCreator.OnSpawnMarkerReleased
                .Subscribe(SpawnInitialPudge)
                .AddTo(ref _disposableBag);
        }

        public void Disable()
        {
            if (!_enabled)
                return;

            _enabled = false;
            _disposableBag.Clear();
        }

        public void SetInitialPudgeState(Pudge.State initialPudgeState)
        {
            _inititalPudgeState = initialPudgeState;
        }

        public void SpawnPudge(Pose pudgePose, Pudge.State pudgeState, float pudgeScale)
        {
            IPudgeView pudgeView = _pudgeSpawnerView.CreatePudgeObject(pudgeState);
            Pudge pudge = new(pudgeView, _tickService);

            pudge.Initialize(pudgePose, pudgeState, pudgeScale);
            _spawnedPudges.Add(pudge);
            _onPudgeSpawned.OnNext(pudge);
        }

        public void DespawnPudge(Pudge pudge)
        {
            pudge.Despawn();
            _spawnedPudges.Remove(pudge);
        }

        private void SpawnInitialPudge(Pose spawnMarkerPose)
        {
            if (_inititalPudgeState == Pudge.State.None)
                return;

            SpawnPudge(
                spawnMarkerPose,
                _inititalPudgeState,
                _pudgeSpawnerView.InitialScale);
        }
    }
}
