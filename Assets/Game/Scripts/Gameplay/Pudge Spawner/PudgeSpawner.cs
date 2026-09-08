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

        public PudgeSpawner(
            InputService inputService,
            ARRaycastService raycastService,
            PudgeSpawnerView pudgeSpawnerView)
        {
            _inputService = inputService;
            _raycastService = raycastService;
            _pudgeSpawnerView = pudgeSpawnerView;
        }

        public void Initialize()
        {
            _inputService.OnInputActionPerformed
                .Where(context => context is
                {
                    ActionType: ActionType.Press,
                    ActionStatus: ActionStatus.Started,
                    IsOverUI: false
                })
                .Subscribe(CreateSpawnMarker);

            _inputService.OnInputActionPerformed
                .Where(context => context is
                {
                    ActionType: ActionType.Drag,
                    ActionStatus: ActionStatus.Performed,
                })
                .Subscribe(MoveSpawnMarker);

            _inputService.OnInputActionPerformed
                .Where(context => context is
                {
                    ActionType: ActionType.Press,
                    ActionStatus: ActionStatus.Canceled,
                })
                .Do(SpawnPudge)
                .Subscribe(DespawnMarker);
        }

        private void CreateSpawnMarker(InputContext context)
        {
            if (!_raycastService.RaycastOnFloor(context.ScreenPosition, out Pose pose))
                return;

            SpawnMarkerView spawnMarkerView = _pudgeSpawnerView.CreateSpawnMarkerObject(pose.position, pose.rotation);
            _spawnMarker = new SpawnMarker(spawnMarkerView, pose.position, pose.rotation);
        }

        private void MoveSpawnMarker(InputContext context)
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

        private void SpawnPudge(InputContext context)
        {
            if (_spawnMarker == null)
                return;

            PudgeView pudgeView = _pudgeSpawnerView.CreatePudgeObject(_spawnMarker.Position, _spawnMarker.Rotation);
            Pudge pudge = new(pudgeView);

            pudge.PlayRandomAnimation();

            _spawnedPudges.Add(pudge);
        }

        private void DespawnMarker(InputContext _)
        {
            if (_spawnMarker == null)
                return;

            _spawnMarker.Delete();
            _spawnMarker = null;
        }

        private void DespawnAllPudges(Unit _)
        {
            foreach (var pudge in _spawnedPudges)
                pudge.Despawn();

            _spawnedPudges.Clear();
        }
    }
}
