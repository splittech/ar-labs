namespace Game.Core
{
    public interface ICrossDetectorView
    {
        float MaxDiagonalDeltaAngle { get; }
        float MaxDeltaTimeBwetweenTwoSwipes { get; }
    }
}
