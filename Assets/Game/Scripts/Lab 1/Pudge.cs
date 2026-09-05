using UnityEngine;

namespace Game.Lab1
{
    public class Pudge : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        public void SetAnimatorIntParameter(string animatorChoiceParameterName, int value)
        {
            animator.SetInteger(animatorChoiceParameterName, value);
        }
    }
}
