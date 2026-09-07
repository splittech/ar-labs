using UnityEngine;

namespace Game.Gameplay
{
    public class PudgeView : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string _animatorChoiceParameterName;
        [SerializeField] private int _numberOfAnimations;

        public void PlayRandomAnimation()
        {
            animator.SetInteger(_animatorChoiceParameterName, Random.Range(0, _numberOfAnimations));
        }

        public void DestroyObject()
        {
            Destroy(gameObject);
        }
    }
}