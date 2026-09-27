using R3;

namespace Game.Core.Input
{
    public class InputLogger
    {
        private readonly InputService _inputService;
        private readonly GameLogger _logger;

        private bool _enabled;

        private DisposableBag _disposableBag;

        public InputLogger(InputService inputService, LoggingService loggingService)
        {
            _inputService = inputService;

            _logger = loggingService.GetLogger(LoggingChannel.InputService);
        }

        public void Enable()
        {
            if (_enabled)
                return;
            _enabled = true;

            _inputService.OnInputActionPerformed
                .Subscribe(context => _logger.Log($"Input action happened, context: {context}."))
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