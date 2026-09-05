using UnityEngine;

namespace Game.Lab1
{
    public class Pudge : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string _animatorChoiceParameterName;
        [SerializeField] private int _numberOfAnimations;

        public void PlayRandomAnimation()
        {
            int randomInt = Random.Range(0, _numberOfAnimations);
            animator.SetInteger(_animatorChoiceParameterName, randomInt);
            Debug.Log(randomInt);
        }
    }
}
