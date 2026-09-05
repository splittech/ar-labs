using UnityEngine;

namespace Game.Lab1
{
    public class PudgeSpawnerView : MonoBehaviour
    {
        [SerializeField] private GameObject _pudgePrefab;
        [SerializeField] private string _animatorChoiceParameterName;

        public Pudge SpawnPudge(Vector3 position, Quaternion rotation)
        {
            GameObject pudgeObject = Instantiate(_pudgePrefab, position, rotation);
            Pudge pudge = pudgeObject.GetComponent<Pudge>();
            pudge.SetAnimatorIntParameter(_animatorChoiceParameterName, Random.Range(0, 10));
            return pudge;
        }

        public void DespawnPudge(Pudge pudge)
        {
            Destroy(pudge.gameObject);
        }
    }
}
