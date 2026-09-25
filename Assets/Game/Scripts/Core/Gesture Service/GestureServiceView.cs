using Lean.Touch;
using R3;
using UnityEngine;

namespace Game.Core
{
    public class GestureServiceView : MonoBehaviour
    {
        [SerializeField] private LeanFingerSwipe _leanFingerSwipe;

        [SerializeField] private float _maxHorizontalDeltaAngle = 30f;
        [SerializeField] private float _maxCrossDeltaAngle = 30f;
        [SerializeField] private float _crossDeltaTime = 1f;

        public float MaxHorizontalDeltaAngle => _maxHorizontalDeltaAngle;
        public float MaxCrossDeltaAngle => _maxCrossDeltaAngle;
        public float CrossDeltaTime => _crossDeltaTime;

        private Subject<LeanFinger> _onSwipe;
        public Observable<LeanFinger> OnSwipe => _onSwipe;

        private void Start()
        {
            _leanFingerSwipe.OnFinger.AddListener(OnFingerSwipe);
        }

        private void OnFingerSwipe(LeanFinger leanFinger)
        {
            _onSwipe.OnNext(leanFinger);
        }
    }
}