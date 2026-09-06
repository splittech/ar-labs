using System.Collections.Generic;
using System.Linq;
using Game.Core.AR;
using Game.Core.Input;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Lab1
{
    public class PudgeSpawner
    {
        private readonly InputService _inputService;
        private readonly ARRaycastService _raycastService;
        private readonly PudgeSpawnerView _pudgeSpawnerView;
        private readonly Button _despawnAllPudgesButton;

        private List<Pudge> _spawnedPudges = new();
        private SpawnMarker _spawnMarker;

        public PudgeSpawner(
            InputService inputService,
            ARRaycastService raycastService,
            PudgeSpawnerView pudgeSpawnerView,
            Button deleteAllPudgesButton)
        {
            _inputService = inputService;
            _raycastService = raycastService;
            _pudgeSpawnerView = pudgeSpawnerView;
            _despawnAllPudgesButton = deleteAllPudgesButton;
        }

        public void Initialize()
        {
            _inputService.OnInputActionPerformed
                .Where(context => context.ActionType == ActionType.DragStarted && !context.IsOverUI)
                .Subscribe(CreateSpawnMarker);

            _inputService.OnInputActionPerformed
                .Where(context => context.ActionType == ActionType.DragHold)
                .Subscribe(MoveSpawnMarker);

            _inputService.OnInputActionPerformed
                .Where(context => context.ActionType == ActionType.DragEnded)
                .Subscribe(SpawnPudge);

            _despawnAllPudgesButton.OnClickAsObservable()
                .Subscribe(DespawnAllPudges);
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

            if (!_raycastService.RaycastOnFloor(context.ScreenPosition, out Pose pose))
                return;

            PudgeView pudgeView = _pudgeSpawnerView.CreatePudgeObject(pose.position, pose.rotation);
            Pudge pudge = new(pudgeView);
            pudge.PlayRandomAnimation();
            _spawnedPudges.Add(pudge);

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
