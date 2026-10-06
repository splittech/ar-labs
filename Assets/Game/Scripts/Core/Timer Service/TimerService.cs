namespace Game.Core
{
    public class TimerService : ITimerService
    {
        private readonly ITickService _tickService;

        public TimerService(ITickService tickService)
        {
            _tickService = tickService;
        }

        public ITimer CreateTimer()
        {
            return new Timer(_tickService);
        }
    }
}