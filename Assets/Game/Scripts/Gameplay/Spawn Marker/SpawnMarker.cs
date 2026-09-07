using UnityEngine;

namespace Game.Gameplay
{
    public class SpawnMarker
    {
        private readonly SpawnMarkerView _spawnMarkerView;

        public Vector3 Position => _spawnMarkerView.Position;
        public Quaternion Rotation => _spawnMarkerView.Rotation;

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
