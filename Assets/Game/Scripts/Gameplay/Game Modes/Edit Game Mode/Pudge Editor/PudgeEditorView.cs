using UnityEngine;

namespace Game.Gameplay
{
    public class PudgeEditorView : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private LayerMask _pudgeInteractableLayer;
        [SerializeField] private float _scaleDelta = 0.5f;
        [SerializeField] private float _rotationDelta = 90f;

        public LayerMask PudgeInteractableLayer => _pudgeInteractableLayer;
        public float ScaleDelta => _scaleDelta;
        public float RotationDelta => _rotationDelta;
    }
}