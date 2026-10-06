using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Game.Core.AR
{
    public class RaycastService : IRaycastService
    {
        private readonly ARRaycastManager _aRRaycastManager;
        private readonly Camera _camera;

        private readonly List<ARRaycastHit> _raycastHits = new();

        public RaycastService(ARRaycastManager aRRaycastManager, Camera camera)
        {
            _aRRaycastManager = aRRaycastManager;
            _camera = camera;
        }

        public bool RaycastOnFloor(Vector2 screenPosition, out Pose pose)
        {
            pose = Pose.identity;

            _aRRaycastManager.Raycast(screenPosition, _raycastHits, TrackableType.PlaneWithinPolygon);
            if (_raycastHits.Count == 0)
                return false;

            ARRaycastHit raycastHit = _raycastHits.First();
            if (!IsFloorHit(raycastHit))
                return false;

            pose = raycastHit.pose;
            return true;
        }

        public bool RaycastOnObject(
            Vector2 screenPosition,
            LayerMask interactableLayer,
            out Collider hitCollider,
            float maxDistance = 100f)
        {
            hitCollider = null;

            if (_camera == null)
                return false;

            Ray ray = _camera.ScreenPointToRay(screenPosition);

            if (!Physics.Raycast(ray, out RaycastHit hit, maxDistance, interactableLayer))
            {
                return false;
            }

            hitCollider = hit.collider;
            return true;
        }

        public bool TryRaycastOnComponent<T>(
            Vector2 screenPosition,
            LayerMask interactableLayer,
            out T component,
            float maxDistance = 100f)
        {
            component = default;

            if (!RaycastOnObject(screenPosition, interactableLayer, out Collider hitCollider, maxDistance))
                return false;

            return hitCollider.TryGetComponent(out component);
        }

        private bool IsFloorHit(ARRaycastHit raycastHit)
        {
            if (raycastHit.trackable is not ARPlane plane)
                return false;

            return plane.alignment == PlaneAlignment.HorizontalUp;
        }
    }
}
