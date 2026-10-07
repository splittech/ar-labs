namespace Game.Core
{
    public interface IHorizontalSwipeDetector
    {
        bool TryDetectHorizontalSwipe(Swipe swipe);
    }
}
