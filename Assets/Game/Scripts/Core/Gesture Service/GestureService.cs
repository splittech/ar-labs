using Lean.Touch;
using R3;
using UnityEngine;

namespace Game.Core
{
    public class GestureService
    {
        private readonly GestureServiceView _gestureServiceView;
        private readonly CrossDetector _crossDetector;
        private readonly HorizontalSwipeDetector _horizontalSwipeDetector;
        private readonly Timer _crossTimer;

        private bool _enabled;

        private DisposableBag _disposableBag;

        private Subject<Swipe> _onHorizontalSwipe;
        public Observable<Swipe> OnHorizontalSwipe => _onHorizontalSwipe;

        private Subject<Vector2> _onCross;
        public Observable<Vector2> OnCross => _onCross;

        public GestureService(GestureServiceView gestureServiceView, TimerService timerService, CrossDetector crossDetector, HorizontalSwipeDetector horizontalSwipeDetector)
        {
            _gestureServiceView = gestureServiceView;
            _crossDetector = crossDetector;
            _horizontalSwipeDetector = horizontalSwipeDetector;

            _crossTimer = timerService.CreateTimer();
        }

        public void Enable()
        {
            if (_enabled)
                return;
            _enabled = true;

            _gestureServiceView.OnSwipe
                .Subscribe(OnFingerSwipe)
                .AddTo(ref _disposableBag);
        }

        public void Disable()
        {
            if (!_enabled)
                return;
            _enabled = false;

            _disposableBag.Clear();
        }

        private void OnFingerSwipe(LeanFinger finger)
        {
            Swipe swipe = new(finger.StartScreenPosition, finger.LastScreenPosition);

            DetectHorizontalSwipe(swipe);
            DetectCross(swipe);
        }

        private Swipe _firstSwipe;

        private void DetectCross(Swipe swipe)
        {
            if (!_crossTimer.Elapsed)
            {
                _firstSwipe = swipe;
                _crossTimer.Reset(_gestureServiceView.CrossDeltaTime);
                return;
            }

            // Timer stop.
            _crossTimer.Reset(_gestureServiceView.CrossDeltaTime);

            bool crossDetected = _crossDetector.DetectCross(
                _firstSwipe,
                swipe,
                _gestureServiceView.MaxCrossDeltaAngle,
                out Vector2 intersection);

            if (crossDetected)
                _onCross.OnNext(intersection);
        }

        private void DetectHorizontalSwipe(Swipe swipe)
        {
            bool horizontalSwipeDetected = _horizontalSwipeDetector.DetectHorizontalSwipe(
                swipe,
                _gestureServiceView.MaxHorizontalDeltaAngle);

            if (horizontalSwipeDetected)
                _onHorizontalSwipe.OnNext(swipe);
        }
    }
}