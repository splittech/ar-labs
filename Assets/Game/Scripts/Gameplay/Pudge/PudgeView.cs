using System;
using R3;
using UnityEngine;

namespace Game.Gameplay
{
    public class PudgeView : MonoBehaviour
    {
        public enum AnimatorState
        {
            Normal,
            Happy,
            Sad
        }

        [Header("General")]
        [SerializeField] private string _name;
        [SerializeField, TextArea] private string _description;

        [Header("Movement Speed")]
        [SerializeField] private float _linearMovementSpeed = 0.1f;
        [SerializeField] private float _dampedInitialMovementSpeed = 0.1f;
        [SerializeField] private float _dampedMovementSpeedLoss = 0.02f;

        [Header("Rotation Speed")]
        [SerializeField] private float _linearRotationSpeed = 500f;
        [SerializeField] private float _dampedInitialRotationSpeed = 500f;
        [SerializeField] private float _dampedRotationSpeedLoss = 0.02f;

        [Header("Scaling Speed")]
        [SerializeField] private float _linearScaleSpeed = 0.2f;
        [SerializeField] private float _dampedInitialScaleSpeed = 0.2f;
        [SerializeField] private float _dampedScaleSpeedLoss = 0.02f;

        [Header("Animator")]
        [SerializeField] private Animator _animator;
        [SerializeField] private string _normalStateParameterName;
        [SerializeField] private string _happyStateParameterName;
        [SerializeField] private string _sadStateParameterName;

        [Header("Selection")]
        [SerializeField] private GameObject _selectionMarker;

        private Pudge _pudge;

        public Pudge Pudge => _pudge;
        public string Name => _name;
        public string Description => _description;

        public float LinearMovementSpeed => _linearMovementSpeed;
        public float DampedInitialMovementSpeed => _dampedInitialMovementSpeed;
        public float DampedMovementSpeedLoss => _dampedMovementSpeedLoss;
        public float LinearRotationSpeed => _linearRotationSpeed;
        public float DampedInitialRotationSpeed => _dampedInitialRotationSpeed;
        public float DampedRotationSpeedLoss => _dampedRotationSpeedLoss;
        public float LinearScaleSpeed => _linearScaleSpeed;
        public float DampedInitialScaleSpeed => _dampedInitialScaleSpeed;
        public float DampedScalingSpeedLoss => _dampedScaleSpeedLoss;

        private Subject<Unit> _onSelected = new();
        public Observable<Unit> OnSelected => _onSelected;

        public void Initialize(Pudge pudge)
        {
            _pudge = pudge;
        }

        public void DestroyObject()
        {
            Destroy(gameObject);
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        public void SetScale(float scale)
        {
            transform.localScale = Vector3.one * scale;
        }

        public void SetAnimatorState(AnimatorState animatorState)
        {
            string parameterName = animatorState switch
            {
                AnimatorState.Normal => _normalStateParameterName,
                AnimatorState.Happy => _happyStateParameterName,
                AnimatorState.Sad => _sadStateParameterName,
                _ => throw new NotImplementedException()
            };

            _animator.SetTrigger(parameterName);
        }

        public void Select()
        {
            _selectionMarker.SetActive(true);
        }

        public void Deselect()
        {
            _selectionMarker.SetActive(false);
        }
    }
}