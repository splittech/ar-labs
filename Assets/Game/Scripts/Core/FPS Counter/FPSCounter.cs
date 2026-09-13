namespace Game.Core
{
    public class FPSCounter
    {
        private readonly FPSCounterConfig _config;
        private readonly FPSCounterView _fpsCounterView;

        private int _accumulatedFrames;
        private float _accumulatedTime;

        public FPSCounter(FPSCounterView fpsCounterView, FPSCounterConfig config)
        {
            _fpsCounterView = fpsCounterView;
            _config = config;
        }

        public void Initialize()
        {
            _fpsCounterView.OnFrameUpdated += OnFrameUpdated;
        }

        private void OnFrameUpdated(float frameDelta)
        {
            _accumulatedTime += frameDelta;
            _accumulatedFrames++;

            if (_accumulatedTime > _config.TimeBetweenFPSTextUpdate)
            {
                float fps = CalculateFPS(_accumulatedFrames, _accumulatedTime);
                _fpsCounterView.ShowFPS(fps);

                _accumulatedFrames = 0;
                _accumulatedTime = 0f;
            }
        }

        private float CalculateFPS(int frames, float time)
        {
            return frames / time;
        }
    }
}