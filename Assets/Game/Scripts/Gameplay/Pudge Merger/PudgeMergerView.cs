using UnityEngine;

namespace Game.Gameplay
{
    public class PudgeMergerView : MonoBehaviour
    {
        [SerializeField] private float _addScale = 1f;
        [SerializeField] private float _scaleToDestroy = 3f;

        public float AddScale => _addScale;
        public float ScaleToDestroy => _scaleToDestroy;
    }
}