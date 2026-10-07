using UnityEngine;

namespace Game.Core
{
    public class HorizontalSwipeDetectorView : MonoBehaviour, IHorizontalSwipeDetectorView
    {
        // Не больше 45° минус MaxDiagonalDeltaAngle у CrossDetectorView, иначе горизонтальный свайп
        // перехватит пологий штрих креста.
        [SerializeField] private float _maxHorizontalDeltaAngle = 20f;

        public float MaxHorizontalDeltaAngle => _maxHorizontalDeltaAngle;
    }
}