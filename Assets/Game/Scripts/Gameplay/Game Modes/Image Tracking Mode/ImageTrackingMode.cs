using Game.Core.AR;

namespace Game.Gameplay
{
    public class ImageTrackingMode : GameMode
    {
        private readonly ARService _ARService;
        private readonly VideoPlayerSpawner _videoPlayerSpawner;

        public ImageTrackingMode(ARService aRService, VideoPlayerSpawner videoPlayerSpawner)
        {
            _ARService = aRService;
            _videoPlayerSpawner = videoPlayerSpawner;
        }

        public override void Enable()
        {
            _videoPlayerSpawner.Enable();

            _ARService.SwitchDetectionType(ARService.DetectionType.Images);
        }

        public override void Disable()
        {
            _videoPlayerSpawner.Disable();
        }
    }
}