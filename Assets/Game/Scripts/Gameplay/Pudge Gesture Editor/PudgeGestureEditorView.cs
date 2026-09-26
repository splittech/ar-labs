using UnityEngine;

namespace Game.Gameplay
{
    public class PudgeGestureEditorView : MonoBehaviour
    {
        [SerializeField] private LayerMask _pudgeLayerMask;

        public LayerMask PudgeLayerMask => _pudgeLayerMask;
    }
}