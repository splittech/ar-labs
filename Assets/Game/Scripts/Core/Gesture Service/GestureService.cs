using R3;
using UnityEngine;

namespace Game.Core
{
    public class GestureService
    {
        private readonly GestureServiceView _gestureServiceView;
        private readonly CrossDetector _crossDetector;
        private readonly HorizontalSwipeDetector _horizontalSwipeDetector;

        private bool _enabled;

        private DisposableBag _disposableBag;

        private Subject<Swipe> _onHorizontalSwipe = new();
        public Observable<Swipe> OnHorizontalSwipe => _onHorizontalSwipe;

        private Subject<Vector2> _onCross = new();
        public Observable<Vector2> OnCross => _onCross;

        public GestureService(
            GestureServiceView gestureServiceView,
            CrossDetector crossDetector,
            HorizontalSwipeDetector horizontalSwipeDetector)
        {
            _gestureServiceView = gestureServiceView;
            _crossDetector = crossDetector;
            _horizontalSwipeDetector = horizontalSwipeDetector;
        }

        public void Enable()
        {
            if (_enabled)
                return;
            _enabled = true;

            _gestureServiceView.OnSwipe
                .Subscribe(DetectGestures)
                .AddTo(ref _disposableBag);
        }

        public void Disable()
        {
            if (!_enabled)
                return;
            _enabled = false;

            _disposableBag.Clear();
        }

        private void DetectGestures(Swipe swipe)
        {
            if (_horizontalSwipeDetector.TryDetectHorizontalSwipe(swipe))
                _onHorizontalSwipe.OnNext(swipe);

            if (_crossDetector.TryDetectCross(swipe, out Vector2 intersection))
                _onCross.OnNext(intersection);
        }
    }
}