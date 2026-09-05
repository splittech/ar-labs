using UnityEngine;

namespace Game.Lab1
{
    public class PudgeSpawnerView : MonoBehaviour
    {
        [SerializeField] private GameObject _pudgePrefab;
        [SerializeField] private string _animatorChoiceParameterName;

        public void SpawnPudge(Vector3 position, Quaternion rotation)
        {
            GameObject pudgeObject = Instantiate(_pudgePrefab, position, rotation);
            Animator animator = pudgeObject.GetComponent<Animator>();
            animator.SetInteger(_animatorChoiceParameterName, Random.Range(0, 10));
        }
    }
}
