using R3;
using UnityEngine;

namespace Game.Core
{
    public class GestureService
    {
        private readonly GestureServiceView _view;
        private readonly CrossDetector _crossDetector;
        private readonly HorizontalSwipeDetector _horizontalSwipeDetector;
        private bool _enabled;

        private DisposableBag _seviceDisposableBag;
        private DisposableBag _markerDisposableBag;

        private Subject<Swipe> _onHorizontalSwipe = new();
        public Observable<Swipe> OnHorizontalSwipe => _onHorizontalSwipe;

        private Subject<Vector2> _onCross = new();
        public Observable<Vector2> OnCross => _onCross;

        public GestureService(
            GestureServiceView gestureServiceView,
            CrossDetector crossDetector,
            HorizontalSwipeDetector horizontalSwipeDetector)
        {
            _view = gestureServiceView;
            _crossDetector = crossDetector;
            _horizontalSwipeDetector = horizontalSwipeDetector;
        }

        public void Enable()
        {
            if (_enabled)
                return;
            _enabled = true;

            _view.OnSwipe
                .Subscribe(DetectGestures)
                .AddTo(ref _seviceDisposableBag);
        }

        public void Disable()
        {
            if (!_enabled)
                return;
            _enabled = false;

            _seviceDisposableBag.Clear();
        }

        private void DetectGestures(Swipe swipe)
        {
            if (_horizontalSwipeDetector.TryDetectHorizontalSwipe(swipe))
            {
                _onHorizontalSwipe.OnNext(swipe);
                return;
            }

            if (_crossDetector.TryDetectCross(swipe, out Vector2 intersection))
            {
                _onCross.OnNext(intersection);
                return;
            }
        }
    }
}