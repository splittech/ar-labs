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
                .Where(context => context.ActionType == ActionType.Tap || !context.IsOverUI)
                .Subscribe(SpawnPudge);

            _despawnAllPudgesButton.OnClickAsObservable()
                .Subscribe(DespawnAllPudges);
        }

        private void SpawnPudge(InputContext context)
        {
            if (!_raycastService.RaycastOnFloor(context.ScreenPosition, out Pose pose))
                return;

            PudgeView pudgeView = _pudgeSpawnerView.CreatePudgeObject(pose.position, pose.rotation);
            Pudge pudge = new(pudgeView);
            pudge.PlayRandomAnimation();

            _spawnedPudges.Add(pudge);
        }

        private void DespawnAllPudges(Unit _)
        {
            foreach (var pudge in _spawnedPudges)
                pudge.Despawn();

            _spawnedPudges.Clear();
        }
    }
}
