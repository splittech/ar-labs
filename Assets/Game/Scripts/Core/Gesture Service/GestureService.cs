using R3;
using UnityEngine;

namespace Game.Core
{
    public class GestureService
    {
        private const int DoubleTapCount = 2;

        private readonly GestureServiceView _view;
        private readonly CrossDetector _crossDetector;
        private readonly HorizontalSwipeDetector _horizontalSwipeDetector;
        private readonly TwistDetector _twistDetector;

        // Сервисом пользуются несколько потребителей, поэтому включение считается по ссылкам.
        private int _enableCount;

        private DisposableBag _seviceDisposableBag;
        private DisposableBag _markerDisposableBag;

        private Subject<Swipe> _onHorizontalSwipe = new();
        public Observable<Swipe> OnHorizontalSwipe => _onHorizontalSwipe;

        private Subject<Vector2> _onCross = new();
        public Observable<Vector2> OnCross => _onCross;

        private Subject<Vector2> _onTap = new();
        public Observable<Vector2> OnTap => _onTap;

        private Subject<Vector2> _onDoubleTap = new();
        public Observable<Vector2> OnDoubleTap => _onDoubleTap;

        private Subject<float> _onTwist = new();
        public Observable<float> OnTwist => _onTwist;

        public GestureService(
            GestureServiceView gestureServiceView,
            CrossDetector crossDetector,
            HorizontalSwipeDetector horizontalSwipeDetector,
            TwistDetector twistDetector)
        {
            _view = gestureServiceView;
            _crossDetector = crossDetector;
            _horizontalSwipeDetector = horizontalSwipeDetector;
            _twistDetector = twistDetector;
        }

        public void Enable()
        {
            _enableCount++;
            if (_enableCount > 1)
                return;

            _view.OnSwipe
                .Subscribe(DetectGestures)
                .AddTo(ref _seviceDisposableBag);

            _view.OnTap
                .Subscribe(finger => DetectTaps(finger.ScreenPosition, finger.TapCount))
                .AddTo(ref _seviceDisposableBag);

            _view.OnTwist
                .Subscribe(DetectTwist)
                .AddTo(ref _seviceDisposableBag);

            _view.OnTwistEnded
                .Subscribe(_ => _twistDetector.Reset())
                .AddTo(ref _seviceDisposableBag);
        }

        public void Disable()
        {
            if (_enableCount == 0)
                return;

            _enableCount--;
            if (_enableCount > 0)
                return;

            _twistDetector.Reset();
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

        private void DetectTaps(Vector2 screenPosition, int tapCount)
        {
            // Каждый тап двойного тапа приходит и как одиночный: первый с TapCount = 1, второй с TapCount = 2.
            _onTap.OnNext(screenPosition);

            if (tapCount == DoubleTapCount)
                _onDoubleTap.OnNext(screenPosition);
        }

        private void DetectTwist(float angleDelta)
        {
            if (_twistDetector.TryDetectTwist(angleDelta))
                _onTwist.OnNext(angleDelta);
        }
    }
}
