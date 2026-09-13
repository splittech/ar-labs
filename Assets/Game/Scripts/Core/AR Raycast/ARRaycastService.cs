using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Game.Core.AR
{
    public class ARRaycastService
    {
        private readonly ARRaycastManager _aRRaycastManager;

        private readonly List<ARRaycastHit> _raycastHits = new();

        public ARRaycastService(ARRaycastManager aRRaycastManager)
        {
            _aRRaycastManager = aRRaycastManager;
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

        private bool IsFloorHit(ARRaycastHit raycastHit)
        {
            if (raycastHit.trackable is not ARPlane plane)
                return false;

            return plane.alignment == PlaneAlignment.HorizontalUp;
        }
    }
}
