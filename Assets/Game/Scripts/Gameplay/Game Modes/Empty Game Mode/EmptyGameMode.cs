using Game.Core.AR;

namespace Game.Gameplay
{
    public class EmptyGameMode : GameMode
    {
        private readonly ARService _ARService;

        public EmptyGameMode(ARService aRService)
        {
            _ARService = aRService;
        }

        public override void Enable()
        {
            _ARService.SwitchDetectionType(ARService.DetectionType.None);
        }

        public override void Disable()
        {
            // Pass.
        }
    }
}