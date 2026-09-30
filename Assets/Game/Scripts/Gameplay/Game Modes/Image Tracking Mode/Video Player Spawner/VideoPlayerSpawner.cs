using System.Collections.Generic;
using Game.Core.AR;
using R3;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Game.Gameplay
{
    public class VideoPlayerSpawner
    {
        private readonly ARService _ARService;
        private readonly VideoPlayerSpawnerView _view;

        private Dictionary<string, VideoPlayerView> _imageVideoPlayers;
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

            _ARService.OnImagesChanged
                .Where(trackables => trackables.removed.Count > 0)
                .SelectMany(trackables => trackables.removed.ToObservable())
                .Subscribe(OnImageRemoved)
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
            _imageVideoPlayers[imageName] = _view.CreateVideoPlayerView(imageName);
        }

        private void OnImageUpdated(ARTrackedImage updatedImage)
        {
            string imageName = updatedImage.referenceImage.name;
            _imageVideoPlayers[imageName].SetPose(updatedImage.pose);
        }

        private void OnImageRemoved(KeyValuePair<TrackableId, ARTrackedImage> removedImage)
        {
            string imageName = removedImage.Value.referenceImage.name;
            _imageVideoPlayers[imageName].Destroy();
            _imageVideoPlayers.Remove(imageName);
        }
    }
}
