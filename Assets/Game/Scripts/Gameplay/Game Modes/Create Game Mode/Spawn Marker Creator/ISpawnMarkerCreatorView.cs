using UnityEngine;

namespace Game.Gameplay
{
    public interface ISpawnMarkerCreatorView
    {
        ISpawnMarkerView CreateSpawnMarkerObject(Vector3 position, Quaternion rotation);
    }
}
