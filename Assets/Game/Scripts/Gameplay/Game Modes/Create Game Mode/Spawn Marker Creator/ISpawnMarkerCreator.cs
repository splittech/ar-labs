using R3;
using UnityEngine;

namespace Game.Gameplay
{
    public interface ISpawnMarkerCreator
    {
        SpawnMarker CurrentSpawnMarker { get; }
        Observable<Pose> OnSpawnMarkerReleased { get; }

        void Enable();
        void Disable();
    }
}
