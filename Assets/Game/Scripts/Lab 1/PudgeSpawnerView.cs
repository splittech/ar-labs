using UnityEngine;

namespace Game.Lab1
{
    public class PudgeSpawnerView : MonoBehaviour
    {
        [SerializeField] private GameObject _pudgePrefab;


        public Pudge SpawnPudge(Vector3 position, Quaternion rotation)
        {
            GameObject pudgeObject = Instantiate(_pudgePrefab, position, rotation);
            Pudge pudge = pudgeObject.GetComponent<Pudge>();
            return pudge;
        }

        public void DespawnPudge(Pudge pudge)
        {
            Destroy(pudge.gameObject);
        }
    }
}
