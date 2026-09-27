using Lean.Touch;
using R3;
using UnityEngine;

namespace Game.Core
{
    public class GestureServiceView : MonoBehaviour
    {
        [SerializeField] private LeanFingerSwipe _leanFingerSwipe;

        private Subject<Swipe> _onSwipe = new();
        public Observable<Swipe> OnSwipe => _onSwipe;

        private void Start()
        {
            _leanFingerSwipe.OnFinger.AddListener(OnFingerSwipe);
        }

        private void OnFingerSwipe(LeanFinger leanFinger)
        {
            Swipe swipe = new(leanFinger.StartScreenPosition, leanFinger.LastScreenPosition);
            _onSwipe.OnNext(swipe);
        }
    }
}