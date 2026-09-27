using System;
using R3;
using UnityEngine;

namespace Game.Core
{
    public class CrossCenterMarkerSpawner
    {
        private readonly GestureService _gestureService;
        private readonly CrossCenterMarkerSpawnerView _view;

        private CrossCenterMarker _crossCenterMarker;
        private bool _enabled;

        private DisposableBag _disposableBag;

        public CrossCenterMarkerSpawner(
            GestureService gestureService,
            CrossCenterMarkerSpawnerView view)
        {
            _gestureService = gestureService;
            _view = view;
        }

        public void Enable()
        {
            if (_enabled)
                return;
            _enabled = true;

            _gestureService.OnCross
                .Do(SpawnMarker)
                .Select(_ => Observable.Timer(TimeSpan.FromSeconds(_view.MarkerShowTime)))
                .Switch()
                .Subscribe(_ => DestroyMarker())
                .AddTo(ref _disposableBag);
        }

        public void Disable()
        {
            if (!_enabled)
                return;
            _enabled = false;

            _disposableBag.Clear();
            DestroyMarker();
        }

        private void SpawnMarker(Vector2 center)
        {
            if (!_view.ShowCrossCenterMarker)
                return;

            DestroyMarker();
            _crossCenterMarker = _view.CreateCrossCenterMarker(center);
        }

        private void DestroyMarker()
        {
            if (_crossCenterMarker == null)
                return;

            _crossCenterMarker.Destroy();
            _crossCenterMarker = null;
        }
    }
}