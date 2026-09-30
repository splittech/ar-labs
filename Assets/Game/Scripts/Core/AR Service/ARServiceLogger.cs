using R3;
using UnityEngine.XR.ARFoundation;

namespace Game.Core.AR
{
    public class ARServiceLogger
    {
        private readonly ARService _ARService;
        private readonly GameLogger _logger;

        private bool _enabled;

        private DisposableBag _disposableBag;

        public ARServiceLogger(ARService aRService, LoggingService loggingService)
        {
            _ARService = aRService;

            _logger = loggingService.GetLogger(LoggingChannel.ARService);
        }

        public void Enable()
        {
            if (_enabled)
                return;
            _enabled = true;

            _ARService.OnPlanesChanged
                .Subscribe(LogPlanes)
                .AddTo(ref _disposableBag);

            _ARService.OnImagesChanged
                .Subscribe(LogImages)
                .AddTo(ref _disposableBag);

            _ARService.OnFacesChanged
                .Subscribe(LogFaces)
                .AddTo(ref _disposableBag);
        }

        private void LogPlanes(ARTrackablesChangedEventArgs<ARPlane> args)
        {
            foreach (var addedPlane in args.added)
                _logger.Log($"Plane added: {addedPlane.name}.");

            // foreach (var updatedPlane in args.updated)
            //     _logger.Log($"Plane updated: {updatedPlane.name}.");

            foreach (var removedPlane in args.removed)
                _logger.Log($"Plane removed: {removedPlane.Value.name}.");
        }

        private void LogImages(ARTrackablesChangedEventArgs<ARTrackedImage> args)
        {
            foreach (var addedImage in args.added)
                _logger.Log($"Image added: {addedImage.name}.");

            // foreach (var updatedImage in args.updated)
            //     _logger.Log($"Image updated: {updatedImage.name}.");

            foreach (var removedImage in args.removed)
                _logger.Log($"Image removed: {removedImage.Value.name}.");
        }

        private void LogFaces(ARTrackablesChangedEventArgs<ARFace> args)
        {
            foreach (var addedFace in args.added)
                _logger.Log($"Face added: {addedFace.name}.");

            // foreach (var updatedFace in args.updated)
            //     _logger.Log($"Face updated: {updatedFace.name}.");

            foreach (var removedFace in args.removed)
                _logger.Log($"Face removed: {removedFace.Value.name}.");
        }

        public void Disable()
        {
            if (!_enabled)
                return;
            _enabled = false;

            _disposableBag.Clear();
        }
    }
}
