using R3;

namespace Game.Core
{
    public class GestureLogger
    {
        private readonly GestureService _gestureService;

        private bool _enabled;

        private DisposableBag _disposableBag;

        public GestureLogger(GestureService gestureService)
        {
            _gestureService = gestureService;
        }

        public void Enable()
        {
            if (_enabled)
                return;

            _enabled = true;


        }

        public void Disable()
        {
            if (!_enabled)
                return;

            _enabled = false;

            _disposableBag.Clear();
        }

        private void OnFingerSwipe(Swipe context)
        {

        }
    }
}