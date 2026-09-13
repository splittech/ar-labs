using System;
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

        [Header("Parameters")]
        [SerializeField] private float _movementSpeed = 0.1f;
        [SerializeField] private float _rotationSpeed = 1337f;

        [Header("Animator")]
        [SerializeField] private Animator _animator;
        [SerializeField] private string _normalStateParameterName;
        [SerializeField] private string _happyStateParameterName;
        [SerializeField] private string _sadStateParameterName;

        public float MovementSpeed => _movementSpeed;
        public float RotationSpeed => _rotationSpeed;

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

        public void DestroyObject()
        {
            Destroy(gameObject);
        }
    }
}