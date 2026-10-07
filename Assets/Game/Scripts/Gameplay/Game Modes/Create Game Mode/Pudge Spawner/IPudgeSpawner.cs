using System.Collections.Generic;
using R3;
using UnityEngine;

namespace Game.Gameplay
{
    public interface IPudgeSpawner
    {
        HashSet<Pudge> SpawnedPudges { get; }
        Observable<Pudge> OnPudgeSpawned { get; }

        void Enable();
        void Disable();
        void SetInitialPudgeState(Pudge.State initialPudgeState);
        void SpawnPudge(Pose pudgePose, Pudge.State pudgeState, float pudgeScale);
        void DespawnPudge(Pudge pudge);
    }
}
