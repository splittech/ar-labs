using UnityEngine;

namespace Game.Lab1
{
    public class PudgeSpawnerView : MonoBehaviour
    {
        [SerializeField] private GameObject _pudgePrefab;
        [SerializeField] private GameObject _spawnMarkerPrefab;

        public PudgeView CreatePudgeObject(Vector3 position, Quaternion rotation)
        {
            GameObject pudgeObject = Instantiate(_pudgePrefab, position, rotation);
            PudgeView pudgeView = pudgeObject.GetComponent<PudgeView>();
            return pudgeView;
        }

        public SpawnMarkerView CreateSpawnMarkerObject(Vector3 position, Quaternion rotation)
        {
            GameObject spawnMarkerObject = Instantiate(_spawnMarkerPrefab, position, rotation);
            SpawnMarkerView spawnMarkerView = spawnMarkerObject.GetComponent<SpawnMarkerView>();
            return spawnMarkerView;
        }
    }
}
