using UnityEngine;

namespace Game.Gameplay
{
    public class SpawnMarkerCreatorView : MonoBehaviour, ISpawnMarkerCreatorView
    {
        [SerializeField] private GameObject _spawnMarkerPrefab;

        public ISpawnMarkerView CreateSpawnMarkerObject(Vector3 position, Quaternion rotation)
        {
            GameObject spawnMarkerObject = Instantiate(_spawnMarkerPrefab, position, rotation);
            ISpawnMarkerView spawnMarkerView = spawnMarkerObject.GetComponent<ISpawnMarkerView>();
            return spawnMarkerView;
        }
    }
}