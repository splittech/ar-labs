using R3;

namespace Game.Core
{
    public class GestureLogger
    {
        private readonly GestureService _gestureService;
        private readonly GameLogger _logger;

        private bool _enabled;

        private DisposableBag _disposableBag;

        public GestureLogger(GestureService gestureService, LoggingService loggingService)
        {
            _gestureService = gestureService;

            _logger = loggingService.GetLogger(LoggingChannel.GestureService);
        }

        public void Enable()
        {
            if (_enabled)
                return;
            _enabled = true;

            _gestureService.OnHorizontalSwipe
                .Subscribe(swipe => _logger.Log($"Horizontal swipe: {swipe}."))
                .AddTo(ref _disposableBag);

            _gestureService.OnCross
                .Subscribe(center => _logger.Log($"Cross: center = {center}."))
                .AddTo(ref _disposableBag);
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