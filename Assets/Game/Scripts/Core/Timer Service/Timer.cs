using R3;

namespace Game.Core
{
    public class Timer
    {
        private readonly TickService _tickService;

        private float _currentTime;

        private DisposableBag _disposableBag;

        private ReactiveProperty<bool> _elapsed = new();
        public ReadOnlyReactiveProperty<bool> Elapsed => _elapsed;

        public Timer(TickService tickService)
        {
            _tickService = tickService;
        }

        public void Reset(float time)
        {
            Stop();

            _currentTime = time;

            _tickService.OnTick
                .Where(tick => tick.Type == TickType.Update)
                .Subscribe(Update)
                .AddTo(ref _disposableBag);
        }

        public void Stop()
        {
            _disposableBag.Clear();
            _elapsed.Value = false;
        }

        private void Update(TickService.Tick tick)
        {
            _currentTime -= tick.DeltaTime;
            if (_currentTime < 0)
            {
                Stop();
                _elapsed.Value = true;
            }
        }
    }
}