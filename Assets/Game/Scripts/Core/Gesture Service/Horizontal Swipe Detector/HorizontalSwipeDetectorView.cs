using UnityEngine;

namespace Game.Core
{
    public class HorizontalSwipeDetectorView : MonoBehaviour
    {
        [SerializeField] private float _maxHorizontalDeltaAngle = 30f;

        public float MaxHorizontalDeltaAngle => _maxHorizontalDeltaAngle;
    }
}