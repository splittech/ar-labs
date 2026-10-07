using R3;
using UnityEngine;

namespace Game.Core
{
    public interface IGestureService
    {
        Observable<Swipe> OnHorizontalSwipe { get; }
        Observable<Vector2> OnCross { get; }

        void Enable();
        void Disable();
    }
}
