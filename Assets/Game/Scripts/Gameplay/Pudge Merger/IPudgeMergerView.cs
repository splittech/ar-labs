using UnityEngine;

namespace Game.Gameplay
{
    public interface IPudgeMergerView
    {
        float AddScale { get; }
        float ScaleToDestroy { get; }

        void CreateFinalEffect(Vector3 position);
    }
}
