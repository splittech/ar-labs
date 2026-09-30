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
        [SerializeField] private ARCameraManager _ARCameraManager;
        [SerializeField] private ARRaycastManager _ARRaycastManager;
        [SerializeField] private ARAnchorManager _ARAnchorManager;

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

        public void DisableAllARManagers()
        {
            _ARPlaneManager.enabled = false;
            _ARTrackedImageManager.enabled = false;
            _ARFaceManager.enabled = false;

            SetPlanesActive(false);
            SetImagesActive(false);
            SetFacesActive(false);

            SwitchCameraFacingDirection(CameraFacingDirection.World);
        }

        public void SwitchToPlaneManager()
        {
            DisableAllARManagers();
            _ARPlaneManager.enabled = true;

            SetPlanesActive(true);
        }

        public void SwitchToTrackedImageManager()
        {
            DisableAllARManagers();
            _ARTrackedImageManager.enabled = true;

            SetImagesActive(true);
        }

        public void SwitchToFaceManager()
        {
            DisableAllARManagers();
            _ARFaceManager.enabled = true;

            SetFacesActive(true);

            SwitchCameraFacingDirection(CameraFacingDirection.User);
        }

        public void Enable()
        {
            enabled = true;
        }

        public void Disable()
        {
            enabled = false;
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

        private void SwitchCameraFacingDirection(CameraFacingDirection cameraFacingDirection)
        {
            bool frontalCamera = cameraFacingDirection == CameraFacingDirection.User;

            _ARRaycastManager.enabled = !frontalCamera;
            _ARAnchorManager.enabled = !frontalCamera;

            _ARCameraManager.requestedFacingDirection = cameraFacingDirection;
        }

        private void SetPlanesActive(bool active)
        {
            foreach (ARPlane plane in _ARPlaneManager.trackables)
                plane.gameObject.SetActive(active);
        }

        private void SetImagesActive(bool active)
        {
            foreach (ARTrackedImage image in _ARTrackedImageManager.trackables)
                image.gameObject.SetActive(active);
        }

        private void SetFacesActive(bool active)
        {
            foreach (ARFace face in _ARFaceManager.trackables)
                face.gameObject.SetActive(active);
        }
    }
}
