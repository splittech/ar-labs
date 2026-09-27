using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Core.Input;
using R3;
using UnityEngine;

namespace Game.Gameplay
{
    public class PudgeSpawner
    {
        private readonly InputService _inputService;
        private readonly PudgeSpawnerView _pudgeSpawnerView;
        private readonly SpawnMarkerCreator _spawnMarkerCreator;
        private readonly TickService _tickService;

        private HashSet<Pudge> _spawnedPudges = new();
        private Pudge.State _inititalPudgeState;
        private bool _enabled;

        private DisposableBag _disposableBag;

        public HashSet<Pudge> SpawnedPudges => _spawnedPudges;

        private Subject<Pudge> _onPudgeSpawned = new();
        public Observable<Pudge> OnPudgeSpawned => _onPudgeSpawned;

        public PudgeSpawner(
            InputService inputService,
            PudgeSpawnerView pudgeSpawnerView,
            SpawnMarkerCreator spawnMarkerCreator,
            TickService tickService)
        {
            _inputService = inputService;
            _pudgeSpawnerView = pudgeSpawnerView;
            _spawnMarkerCreator = spawnMarkerCreator;
            _tickService = tickService;
        }

        public void Enable()
        {
            if (_enabled)
                return;

            _enabled = true;

            _inputService.OnInputActionPerformed
                .Where(context => context is
                {
                    ActionType: ActionType.Press,
                    ActionStatus: ActionStatus.Canceled,
                })
                .Subscribe(_ => SpawnInitialPudge())
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
            PudgeView pudgeView = _pudgeSpawnerView.CreatePudgeObject(pudgeState);
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

        private void SpawnInitialPudge()
        {
            if (_spawnMarkerCreator.CurrentSpawnMarker == null || _inititalPudgeState == Pudge.State.None)
                return;

            SpawnPudge(
                _spawnMarkerCreator.CurrentSpawnMarker.Pose,
                _inititalPudgeState,
                _pudgeSpawnerView.InitialScale);
        }
    }
}
