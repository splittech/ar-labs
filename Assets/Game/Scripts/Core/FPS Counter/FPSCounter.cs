using R3;

namespace Game.Core
{
    public class FPSCounter
    {
        private readonly FPSCounterView _fpsCounterView;
        private readonly TickService _tickService;

        private int _accumulatedFrames;
        private float _accumulatedTime;
        private bool _enabled;

        private DisposableBag _disposableBag;

        public FPSCounter(FPSCounterView fpsCounterView, TickService tickService)
        {
            _fpsCounterView = fpsCounterView;
            _tickService = tickService;
        }

        public void Enable()
        {
            if (_enabled)
                return;
            _enabled = true;

            _tickService.OnTick
                .Where(tick => tick.Type == TickType.Update)
                .Subscribe(AccamulateFrame)
                .AddTo(ref _disposableBag);
        }

        public void Disable()
        {
            if (!_enabled)
                return;
            _enabled = false;

            _disposableBag.Clear();
        }

        private void AccamulateFrame(TickService.Tick tick)
        {
            _accumulatedTime += tick.DeltaTime;
            _accumulatedFrames++;

            if (_accumulatedTime < _fpsCounterView.TimeBetweenFPSTextUpdate)
                return;

            float fps = CalculateFPS(_accumulatedFrames, _accumulatedTime);
            _fpsCounterView.ShowFPS(fps);

            _accumulatedFrames = 0;
            _accumulatedTime = 0f;
        }

        private float CalculateFPS(int frames, float time)
        {
            return frames / time;
        }
    }
}