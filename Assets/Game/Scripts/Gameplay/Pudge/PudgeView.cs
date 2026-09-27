using System;
using System.Collections.Generic;
using DG.Tweening;
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
        [SerializeField] private float _dampedInitialMovementSpeed = 1f;
        [SerializeField] private float _dampedMinimumMovementSpeed = 0.02f;
        [SerializeField] private float _dampedMovementSharpness = 4f;

        [Header("Rotation Speed")]
        [SerializeField] private float _linearRotationSpeed = 500f;
        [SerializeField] private float _dampedInitialRotationSpeed = 700f;
        [SerializeField] private float _dampedMinimumRotationSpeed = 30f;
        [SerializeField] private float _dampedRotationSharpness = 4f;

        [Header("Scaling Speed")]
        [SerializeField] private float _linearScaleSpeed = 0.2f;
        [SerializeField] private float _dampedInitialScalingSpeed = 1f;
        [SerializeField] private float _dampedMinimumScalingSpeed = 0.05f;
        [SerializeField] private float _dampedScalingSharpness = 4f;

        [Header("Animator")]
        [SerializeField] private Animator _animator;
        [SerializeField] private string _normalStateParameterName;
        [SerializeField] private string _happyStateParameterName;
        [SerializeField] private string _sadStateParameterName;

        [Header("Renderer")]
        [SerializeField] private List<Renderer> _renderers;

        [Header("Selection")]
        [SerializeField] private GameObject _selectionMarker;

        private Pudge _pudge;
        private Sequence _alphaAnimation;

        public Pudge Pudge => _pudge;
        public string Name => _name;
        public string Description => _description;

        public float LinearMovementSpeed => _linearMovementSpeed;
        public float DampedInitialMovementSpeed => _dampedInitialMovementSpeed;
        public float DampedMovementSharpness => _dampedMovementSharpness;
        public float DampedMinimumMovementSpeed => _dampedMinimumMovementSpeed;

        public float LinearRotationSpeed => _linearRotationSpeed;
        public float DampedInitialRotationSpeed => _dampedInitialRotationSpeed;
        public float DampedRotationSharpness => _dampedRotationSharpness;
        public float DampedMinimumRotationSpeed => _dampedMinimumRotationSpeed;

        public float LinearScaleSpeed => _linearScaleSpeed;
        public float DampedInitialScalingSpeed => _dampedInitialScalingSpeed;
        public float DampedScalingSharpness => _dampedScalingSharpness;
        public float DampedMinimumScalingSpeed => _dampedMinimumScalingSpeed;

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

        public void ChangeAlphaTo(float targetAlpha)
        {
            _alphaAnimation?.Kill();
            _alphaAnimation = DOTween.Sequence().SetLink(gameObject);

            float duration = transform.localScale.x / _linearScaleSpeed;

            foreach (Renderer renderer in _renderers)
                _alphaAnimation.Join(renderer.material.DOFade(targetAlpha, duration));
        }
    }
}