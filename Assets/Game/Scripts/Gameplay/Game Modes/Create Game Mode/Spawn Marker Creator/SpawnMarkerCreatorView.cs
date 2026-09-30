using UnityEngine;

namespace Game.Gameplay
{
    public class SpawnMarkerCreatorView : MonoBehaviour
    {
        [SerializeField] private GameObject _spawnMarkerPrefab;

        public SpawnMarkerView CreateSpawnMarkerObject(Vector3 position, Quaternion rotation)
        {
            GameObject spawnMarkerObject = Instantiate(_spawnMarkerPrefab, position, rotation);
            SpawnMarkerView spawnMarkerView = spawnMarkerObject.GetComponent<SpawnMarkerView>();
            return spawnMarkerView;
        }
    }
}