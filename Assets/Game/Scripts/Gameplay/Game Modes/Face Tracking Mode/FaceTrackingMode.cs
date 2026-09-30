using Game.Core.AR;

namespace Game.Gameplay
{
    public class FaceTrackingMode : GameMode
    {
        private readonly ARService _ARService;
        private readonly FaceSpawner _faceSpawner;

        public FaceTrackingMode(ARService aRService, FaceSpawner faceSpawner)
        {
            _ARService = aRService;
            _faceSpawner = faceSpawner;
        }

        public override void Enable()
        {
            _faceSpawner.Enable();

            _ARService.SwitchDetectionType(ARService.DetectionType.Faces);
        }

        public override void Disable()
        {
            _faceSpawner.Disable();
        }
    }
}