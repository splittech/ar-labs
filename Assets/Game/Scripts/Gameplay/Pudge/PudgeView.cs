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

        [Header("Animator")]
        [SerializeField] private Animator _animator;
        [SerializeField] private string _normalStateParameterName;
        [SerializeField] private string _happyStateParameterName;
        [SerializeField] private string _sadStateParameterName;

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

        public void DestroyObject()
        {
            Destroy(gameObject);
        }
    }
}