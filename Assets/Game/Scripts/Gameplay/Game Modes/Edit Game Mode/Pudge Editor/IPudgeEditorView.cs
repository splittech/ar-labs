using UnityEngine;

namespace Game.Gameplay
{
    public interface IPudgeEditorView
    {
        LayerMask PudgeInteractableLayer { get; }
        float ScaleDelta { get; }
        float RotationDelta { get; }
    }
}
