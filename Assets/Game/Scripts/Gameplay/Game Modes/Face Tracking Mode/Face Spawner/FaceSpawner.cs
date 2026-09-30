using System;
using System.Collections.Generic;
using Game.Core.AR;
using R3;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace Game.Gameplay
{
    public class FaceSpawner
    {
        private readonly ARService _ARService;

        private bool _enabled;

        private DisposableBag _disposableBag;

        public FaceSpawner(ARService aRService)
        {
            _ARService = aRService;
        }

        public void Enable()
        {
            if (_enabled)
                return;
            _enabled = true;

            _ARService.OnFacesChanged
                .Where(trackables => trackables.added.Count > 0)
                .SelectMany(trackables => trackables.added.ToObservable())
                .Subscribe(OnFaceAdded)
                .AddTo(ref _disposableBag);

            _ARService.OnFacesChanged
                .Where(trackables => trackables.updated.Count > 0)
                .SelectMany(trackables => trackables.updated.ToObservable())
                .Subscribe(OnFaceUpdated)
                .AddTo(ref _disposableBag);

            _ARService.OnFacesChanged
                .Where(trackables => trackables.removed.Count > 0)
                .SelectMany(trackables => trackables.removed.ToObservable())
                .Subscribe(OnFaceRemoved)
                .AddTo(ref _disposableBag);
        }

        public void Disable()
        {
            if (!_enabled)
                return;
            _enabled = false;

            _disposableBag.Clear();
        }

        private void OnFaceAdded(ARFace face)
        {
            throw new NotImplementedException();
        }

        private void OnFaceUpdated(ARFace face)
        {
            throw new NotImplementedException();
        }

        private void OnFaceRemoved(KeyValuePair<TrackableId, ARFace> pair)
        {
            throw new NotImplementedException();
        }
    }
}
