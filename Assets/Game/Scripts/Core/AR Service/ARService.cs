using System;
using R3;
using UnityEngine.XR.ARFoundation;

namespace Game.Core.AR
{
    public class ARService
    {
        public enum DetectionType
        {
            None,
            Planes,
            Images,
            Faces
        }

        private readonly ARServiceView _view;

        private bool _enabled;

        public Subject<ARTrackablesChangedEventArgs<ARPlane>> OnPlanesChanged => _view.OnPlanesChanged;
        public Subject<ARTrackablesChangedEventArgs<ARTrackedImage>> OnImagesChanged => _view.OnImagesChanged;
        public Subject<ARTrackablesChangedEventArgs<ARFace>> OnFacesChanged => _view.OnFacesChanged;

        public ARService(ARServiceView view)
        {
            _view = view;
        }

        public void Enable()
        {
            if (_enabled)
                return;
            _enabled = true;

            _view.Enable();
        }

        public void Disable()
        {
            if (!_enabled)
                return;
            _enabled = false;

            _view.Disable();
        }

        public void SwitchDetectionType(DetectionType detectionType)
        {
            Action switchAction = detectionType switch
            {
                DetectionType.None => _view.DisableAllARManagers,
                DetectionType.Planes => _view.SwitchToPlaneManager,
                DetectionType.Images => _view.SwitchToTrackedImageManager,
                DetectionType.Faces => _view.SwitchToFaceManager,
                _ => throw new NotImplementedException()
            };

            switchAction.Invoke();
        }
    }
}
