using UnityEngine;

namespace Game.Core
{
    public interface ICrossDetector
    {
        bool TryDetectCross(Swipe newSwipe, out Vector2 intersection);
    }
}
