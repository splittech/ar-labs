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

        private readonly Dictionary<string, VideoPlayerView> _imageVideoPlayers = new();
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
                .SelectMany(trackables => trackables.removed.ToObservable())
                .Subscribe(pair => OnImageRemoved(pair.Value))
                .AddTo(ref _disposableBag);

            _ARService.OnImagesChanged
                .SelectMany(trackables => trackables.added.ToObservable())
                .Subscribe(OnImageAddedOrUpdated)
                .AddTo(ref _disposableBag);

            _ARService.OnImagesChanged
                .SelectMany(trackables => trackables.updated.ToObservable())
                .Subscribe(OnImageAddedOrUpdated)
                .AddTo(ref _disposableBag);
        }

        public void Disable()
        {
            if (!_enabled)
                return;
            _enabled = false;

            _disposableBag.Clear();

            foreach (VideoPlayerView videoPlayerView in _imageVideoPlayers.Values)
                videoPlayerView.Destroy();
            _imageVideoPlayers.Clear();
        }

        private void OnImageAddedOrUpdated(ARTrackedImage image)
        {
            string imageName = image.referenceImage.name;

            if (!_imageVideoPlayers.TryGetValue(imageName, out VideoPlayerView videoPlayerView))
            {
                videoPlayerView = _view.CreateVideoPlayerView(imageName);
                _imageVideoPlayers[imageName] = videoPlayerView;
            }

            bool isTracking = image.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking;

            videoPlayerView.SetActive(isTracking);

            if (isTracking)
                videoPlayerView.SetPose(image.pose);
        }

        private void OnImageRemoved(ARTrackedImage image)
        {
            if (!_imageVideoPlayers.Remove(image.referenceImage.name, out VideoPlayerView videoPlayerView))
                return;

            videoPlayerView.Destroy();
        }
    }
}