using UnityEngine;

namespace Game.Gameplay
{
    public class SpawnMarker
    {
        private readonly ISpawnMarkerView _spawnMarkerView;

        public Pose Pose => new(_spawnMarkerView.Position, _spawnMarkerView.Rotation);

        public SpawnMarker(ISpawnMarkerView spawnMarkerView, Pose pose)
        {
            _spawnMarkerView = spawnMarkerView;

            SetPositionAndRotation(pose.position, pose.rotation);
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
