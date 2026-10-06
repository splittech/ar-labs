namespace Game.Core
{
    public class TimerService
    {
        private readonly ITickService _tickService;

        public TimerService(ITickService tickService)
        {
            _tickService = tickService;
        }

        public Timer CreateTimer()
        {
            return new Timer(_tickService);
        }
    }
}