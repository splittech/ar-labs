using System.Collections.Generic;
using Game.Core.AR;
using R3;
using UnityEngine.XR.ARFoundation;

namespace Game.Gameplay
{
    public class VideoPlayerSpawner
    {
        private readonly ARService _ARService;
        private readonly VideoPlayerSpawnerView _view;

        private Dictionary<string, VideoPlayerView> _imageVideoPlayers = new();
        private bool _enabled;

        private DisposableBag _disposableBag;

        public VideoPlayerSpawner(ARService aRService, VideoPlayerSpawnerView view)
        {
            _ARService = aRService;
            _view = view;
        }

        public void Enable()
        {
            if (_enabled)
                return;
            _enabled = true;

            _ARService.OnImagesChanged
                .Where(trackables => trackables.added.Count > 0)
                .SelectMany(trackables => trackables.added.ToObservable())
                .Subscribe(OnImageAdded)
                .AddTo(ref _disposableBag);

            _ARService.OnImagesChanged
                .Where(trackables => trackables.updated.Count > 0)
                .SelectMany(trackables => trackables.updated.ToObservable())
                .Subscribe(OnImageUpdated)
                .AddTo(ref _disposableBag);
        }

        public void Disable()
        {
            if (!_enabled)
                return;
            _enabled = false;

            _disposableBag.Clear();

            foreach (var videoPlayer in _imageVideoPlayers)
                videoPlayer.Value.Destroy();
            _imageVideoPlayers.Clear();
        }

        private void OnImageAdded(ARTrackedImage addedImage)
        {
            string imageName = addedImage.referenceImage.name;

            VideoPlayerView videoPlayerView = _view.CreateVideoPlayerView(imageName);
            _imageVideoPlayers[imageName] = videoPlayerView;

            ApplyTrackingState(videoPlayerView, addedImage);
        }

        private void OnImageUpdated(ARTrackedImage updatedImage)
        {
            string imageName = updatedImage.referenceImage.name;

            if (!_imageVideoPlayers.TryGetValue(imageName, out VideoPlayerView videoPlayerView))
            {
                OnImageAdded(updatedImage);
                return;
            }

            ApplyTrackingState(videoPlayerView, updatedImage);
        }

        private void ApplyTrackingState(VideoPlayerView videoPlayerView, ARTrackedImage image)
        {
            bool isTracking = image.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking;

            videoPlayerView.SetActive(isTracking);

            if (isTracking)
                videoPlayerView.SetPose(image.pose);
        }
    }
}
