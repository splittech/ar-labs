using UnityEngine;

namespace Game.Core
{
    public class CrossDetectorView : MonoBehaviour
    {
        [SerializeField] private float _maxDiagonalDeltaAngle = 30f;
        [SerializeField] private float _maxDeltaTimeBwetweenTwoSwipes = 1f;

        public float MaxDiagonalDeltaAngle => _maxDiagonalDeltaAngle;
        public float MaxDeltaTimeBwetweenTwoSwipes => _maxDeltaTimeBwetweenTwoSwipes;
    }
}