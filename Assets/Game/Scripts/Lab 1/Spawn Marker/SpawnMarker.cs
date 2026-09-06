using UnityEngine;

namespace Game.Lab1
{
    public class SpawnMarker
    {
        private readonly SpawnMarkerView _spawnMarkerView;

        public SpawnMarker(SpawnMarkerView spawnMarkerView, Vector3 position, Quaternion rotation)
        {
            _spawnMarkerView = spawnMarkerView;

            SetPositionAndRotation(position, rotation);
        }

        public void SetPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            _spawnMarkerView.SetTransformPositionAndRotation(position, rotation);
        }

        public void Delete()
        {
            _spawnMarkerView.DestroyObject();
        }
    }
}
