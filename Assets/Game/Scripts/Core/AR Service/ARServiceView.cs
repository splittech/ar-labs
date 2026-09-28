using R3;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Game.Core.AR
{
    public class ARServiceView : MonoBehaviour
    {
        [SerializeField] private ARPlaneManager _ARPlaneManager;
        [SerializeField] private ARTrackedImageManager _ARTrackedImageManager;
        [SerializeField] private ARFaceManager _ARFaceManager;

        private Subject<ARTrackablesChangedEventArgs<ARPlane>> _onPlanesChanged = new();
        private Subject<ARTrackablesChangedEventArgs<ARTrackedImage>> _onImagesChanged = new();
        private Subject<ARTrackablesChangedEventArgs<ARFace>> _onFacesChanged = new();

        public Subject<ARTrackablesChangedEventArgs<ARPlane>> OnPlanesChanged => _onPlanesChanged;
        public Subject<ARTrackablesChangedEventArgs<ARTrackedImage>> OnImagesChanged => _onImagesChanged;
        public Subject<ARTrackablesChangedEventArgs<ARFace>> OnFacesChanged => _onFacesChanged;

        private void OnEnable()
        {
            _ARPlaneManager.trackablesChanged.AddListener(PlaneTrackablesChanged);
            _ARTrackedImageManager.trackablesChanged.AddListener(ImageTrackablesChanged);
            _ARFaceManager.trackablesChanged.AddListener(FaceTrackablesChanged);
        }

        private void OnDisable()
        {
            _ARPlaneManager.trackablesChanged.RemoveListener(PlaneTrackablesChanged);
            _ARTrackedImageManager.trackablesChanged.RemoveListener(ImageTrackablesChanged);
            _ARFaceManager.trackablesChanged.RemoveListener(FaceTrackablesChanged);
        }

        private void PlaneTrackablesChanged(ARTrackablesChangedEventArgs<ARPlane> args)
        {
            _onPlanesChanged.OnNext(args);
        }

        private void ImageTrackablesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args)
        {
            _onImagesChanged.OnNext(args);
        }

        private void FaceTrackablesChanged(ARTrackablesChangedEventArgs<ARFace> args)
        {
            _onFacesChanged.OnNext(args);
        }

        public void DisableAllManagers()
        {
            _ARPlaneManager.enabled = false;
            _ARTrackedImageManager.enabled = false;
            _ARFaceManager.enabled = false;
        }

        public void EnablePlaneManager()
        {
            DisableAllManagers();
            _ARPlaneManager.enabled = true;
        }

        public void EnableTrackedImageManager()
        {
            DisableAllManagers();
            _ARTrackedImageManager.enabled = true;
        }

        public void EnableFaceManager()
        {
            DisableAllManagers();
            _ARFaceManager.enabled = true;
        }

        public void Enable()
        {
            enabled = true;
        }

        public void Disable()
        {
            enabled = false;
        }
    }
}
