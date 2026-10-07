using R3;

namespace Game.Core
{
    public interface IGestureServiceView
    {
        Observable<Swipe> OnSwipe { get; }
    }
}
