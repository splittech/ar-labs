namespace Game.Core
{
    public class TimerService
    {
        private readonly TickService _tickService;

        public TimerService(TickService tickService)
        {
            _tickService = tickService;
        }

        public Timer CreateTimer()
        {
            return new Timer(_tickService);
        }
    }
}