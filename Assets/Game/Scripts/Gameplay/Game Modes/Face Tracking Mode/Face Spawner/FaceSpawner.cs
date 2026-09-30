using Game.Core.AR;
using R3;
using UnityEngine.XR.ARFoundation;

namespace Game.Gameplay
{
    public class FaceSpawner
    {
        private readonly ARService _ARService;
        private readonly FaceSpawnerView _view;

        private ARFaceView _ARFaceView;
        private bool _enabled;

        private DisposableBag _disposableBag;

        public FaceSpawner(ARService aRService, FaceSpawnerView view)
        {
            _ARService = aRService;
            _view = view;
        }

        public void Enable()
        {
            if (_enabled)
                return;
            _enabled = true;

            _ARFaceView = _view.CreateARFaceView();
            _ARFaceView.SetActive(false);

            _ARService.OnFacesChanged
                .Where(trackables => trackables.removed.Count > 0)
                .Subscribe(_ => _ARFaceView.SetActive(false))
                .AddTo(ref _disposableBag);

            _ARService.OnFacesChanged
                .SelectMany(trackables => trackables.added.ToObservable())
                .Subscribe(ApplyTrackingState)
                .AddTo(ref _disposableBag);

            _ARService.OnFacesChanged
                .SelectMany(trackables => trackables.updated.ToObservable())
                .Subscribe(ApplyTrackingState)
                .AddTo(ref _disposableBag);
        }

        public void Disable()
        {
            if (!_enabled)
                return;
            _enabled = false;

            _disposableBag.Clear();

            _ARFaceView.Destroy();
            _ARFaceView = null;
        }

        private void ApplyTrackingState(ARFace face)
        {
            bool isTracking = face.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking;

            _ARFaceView.SetActive(isTracking);

            if (isTracking)
                _ARFaceView.SetPose(face.pose);
        }
    }
}