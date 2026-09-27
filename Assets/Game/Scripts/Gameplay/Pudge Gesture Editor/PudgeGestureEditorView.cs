using UnityEngine;

namespace Game.Gameplay
{
    public class PudgeGestureEditorView : MonoBehaviour
    {
        [SerializeField] private LayerMask _pudgeLayerMask;
        [SerializeField] private float maxSwipeRotationAngle = 360f;

        public LayerMask PudgeLayerMask => _pudgeLayerMask;
        public float MaxSwipeRotationAngle => maxSwipeRotationAngle;
    }
}