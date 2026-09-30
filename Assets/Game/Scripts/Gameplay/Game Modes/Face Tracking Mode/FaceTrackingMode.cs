using Game.Core.AR;

namespace Game.Gameplay
{
    public class FaceTrackingMode : GameMode
    {
        private readonly ARService _ARService;

        public FaceTrackingMode(ARService aRService)
        {
            _ARService = aRService;
        }

        public override void Enable()
        {
            _ARService.SwitchDetectionType(ARService.DetectionType.Faces);
        }

        public override void Disable()
        {

        }
    }
}