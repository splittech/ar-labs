using UnityEngine;

namespace Game.Gameplay
{
    public interface ISpawnMarkerView
    {
        Vector3 Position { get; }
        Quaternion Rotation { get; }

        void SetTransformPositionAndRotation(Vector3 position, Quaternion rotation);
        void DestroyObject();
    }
}
