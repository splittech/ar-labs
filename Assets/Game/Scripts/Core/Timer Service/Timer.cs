using R3;

namespace Game.Core
{
    public class Timer
    {
        private readonly TickService _tickService;

        private float _currentTime;
        private bool _elapsed;

        private DisposableBag _disposableBag;

        public bool Elapsed => _elapsed;

        public Subject<Unit> _onElapsed;
        public Observable<Unit> OnElapsed => _onElapsed;

        public Timer(TickService tickService)
        {
            _tickService = tickService;
        }

        public void Reset(float time)
        {
            Stop();

            _currentTime = time;
            _elapsed = false;

            _tickService.OnTick
                .Where(tick => tick.Type == TickType.Update)
                .Subscribe(Update)
                .AddTo(ref _disposableBag);
        }

        private void Stop()
        {
            _disposableBag.Clear();
            _elapsed = false;
        }

        private void Update(TickService.Tick tick)
        {
            _currentTime -= tick.DeltaTime;
            if (_currentTime < 0)
                Elapse();
        }

        private void Elapse()
        {
            Stop();
            _onElapsed.OnNext(Unit.Default);
        }
    }
}