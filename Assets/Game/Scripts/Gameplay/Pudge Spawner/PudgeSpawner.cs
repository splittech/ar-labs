using System.Collections.Generic;
using System.Linq;
using Game.Core.AR;
using Game.Core.Input;
using R3;
using UnityEngine;

namespace Game.Gameplay
{
    public class PudgeSpawner
    {
        private readonly InputService _inputService;
        private readonly ARRaycastService _raycastService;
        private readonly PudgeSpawnerView _pudgeSpawnerView;

        private List<Pudge> _spawnedPudges = new();
        private SpawnMarker _spawnMarker;
        private Pudge.State _inititalPudgeState;
        private bool _enabled;

        private DisposableBag _disposableBag;

        public Pudge.State InititalPudgeState { set => _inititalPudgeState = value; }

        public PudgeSpawner(
            InputService inputService,
            ARRaycastService raycastService,
            PudgeSpawnerView pudgeSpawnerView)
        {
            _inputService = inputService;
            _raycastService = raycastService;
            _pudgeSpawnerView = pudgeSpawnerView;
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
                    ActionStatus: ActionStatus.Started,
                    IsOverUI: false
                })
                .Subscribe(CreateSpawnMarker)
                .AddTo(ref _disposableBag);

            _inputService.OnInputActionPerformed
                .Where(context => context is
                {
                    ActionType: ActionType.Drag,
                    ActionStatus: ActionStatus.Performed,
                })
                .Subscribe(MoveMarker)
                .AddTo(ref _disposableBag);

            _inputService.OnInputActionPerformed
                .Where(context => context is
                {
                    ActionType: ActionType.Press,
                    ActionStatus: ActionStatus.Canceled,
                })
                .Do(SpawnPudge)
                .Subscribe(_ => DeleteMarker())
                .AddTo(ref _disposableBag);
        }

        public void Disable()
        {
            if (!_enabled)
                return;

            _enabled = false;
            _disposableBag.Clear();

            DeleteMarker();
        }

        private void CreateSpawnMarker(InputContext context)
        {
            if (!_raycastService.RaycastOnFloor(context.ScreenPosition, out Pose pose))
                return;

            SpawnMarkerView spawnMarkerView = _pudgeSpawnerView.CreateSpawnMarkerObject(pose.position, pose.rotation);
            _spawnMarker = new SpawnMarker(spawnMarkerView, pose.position, pose.rotation);
        }

        private void MoveMarker(InputContext context)
        {
            if (_spawnMarker == null)
                return;

            if (context.IsOverUI)
            {
                _spawnMarker.Delete();
                _spawnMarker = null;
                return;
            }

            if (!_raycastService.RaycastOnFloor(context.ScreenPosition, out Pose pose))
                return;

            _spawnMarker.SetPositionAndRotation(pose.position, pose.rotation);
        }

        private void DeleteMarker()
        {
            if (_spawnMarker == null)
                return;

            _spawnMarker.Delete();
            _spawnMarker = null;
        }

        private void SpawnPudge(InputContext context)
        {
            if (_spawnMarker == null || _inititalPudgeState == Pudge.State.None)
                return;

            PudgeView pudgeView = _pudgeSpawnerView.CreatePudgeObject(_spawnMarker.Position, _spawnMarker.Rotation);
            Pudge pudge = new(pudgeView);

            pudge.SetState(_inititalPudgeState);

            _spawnedPudges.Add(pudge);
        }
    }
}
