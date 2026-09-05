using System.Collections.Generic;
using System.Linq;
using Game.Core;
using R3;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Game.Lab1
{
    public class PudgeSpawner
    {
        private readonly InputService _inputService;
        private readonly ARRaycastManager _raycastManager;
        private readonly PudgeSpawnerView _pudgeSpawnerView;

        public PudgeSpawner(InputService inputService, ARRaycastManager raycastManager, PudgeSpawnerView pudgeSpawnerView)
        {
            _inputService = inputService;
            _raycastManager = raycastManager;
            _pudgeSpawnerView = pudgeSpawnerView;
        }

        public void Initialize()
        {
            _inputService.OnTap.Subscribe(SpawnPudge);
        }

        private void SpawnPudge(Vector2 tapScreenPosition)
        {
            List<ARRaycastHit> raycastHits = new();
            _raycastManager.Raycast(tapScreenPosition, raycastHits, TrackableType.Planes);
            if (raycastHits.Count == 0)
                return;

            Pose hitPose = raycastHits.First().pose;
            _pudgeSpawnerView.SpawnPudge(hitPose.position, hitPose.rotation);
        }
    }
}
