using Game.Core.AR;

namespace Game.Gameplay
{
    public class ImageTrackingMode : GameMode
    {
        private readonly ARService _ARService;

        public ImageTrackingMode(ARService aRService)
        {
            _ARService = aRService;
        }

        public override void Enable()
        {
            _ARService.SwitchDetectionType(ARService.DetectionType.Images);
        }

        public override void Disable()
        {

        }
    }
}