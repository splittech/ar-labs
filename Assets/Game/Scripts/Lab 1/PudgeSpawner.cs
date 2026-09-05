using System.Collections.Generic;
using System.Linq;
using Game.Core;
using R3;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Game.Lab1
{
    public class PudgeSpawner
    {
        private readonly InputService _inputService;
        private readonly ARRaycastManager _raycastManager;
        private readonly PudgeSpawnerView _pudgeSpawnerView;
        private readonly Button _despawnAllPudgesButton;

        private List<Pudge> _spawnedPudges = new();

        public PudgeSpawner(
            InputService inputService,
            ARRaycastManager raycastManager,
            PudgeSpawnerView pudgeSpawnerView,
            Button deleteAllPudgesButton)
        {
            _inputService = inputService;
            _raycastManager = raycastManager;
            _pudgeSpawnerView = pudgeSpawnerView;
            _despawnAllPudgesButton = deleteAllPudgesButton;
        }

        public void Initialize()
        {
            _inputService.OnTap.Subscribe(SpawnPudge);
            _despawnAllPudgesButton.OnClickAsObservable().Subscribe(DespawnAllPudges);
        }

        private void SpawnPudge(Vector2 tapScreenPosition)
        {
            List<ARRaycastHit> raycastHits = new();
            _raycastManager.Raycast(tapScreenPosition, raycastHits, TrackableType.PlaneWithinPolygon);
            if (raycastHits.Count == 0)
                return;

            ARRaycastHit raycastHit = raycastHits.First();
            if (!IsFloorHit(raycastHit))
                return;

            Pudge pudge = _pudgeSpawnerView.SpawnPudge(raycastHit.pose.position, raycastHit.pose.rotation);
            pudge.PlayRandomAnimation();

            _spawnedPudges.Add(pudge);
        }

        private void DespawnAllPudges(Unit _)
        {
            foreach (var pudge in _spawnedPudges)
                _pudgeSpawnerView.DespawnPudge(pudge);

            _spawnedPudges.Clear();
        }

        private bool IsFloorHit(ARRaycastHit raycastHit)
        {
            if (raycastHit.trackable is not ARPlane plane)
                return false;

            return plane.alignment == PlaneAlignment.HorizontalUp;
        }
    }
}
