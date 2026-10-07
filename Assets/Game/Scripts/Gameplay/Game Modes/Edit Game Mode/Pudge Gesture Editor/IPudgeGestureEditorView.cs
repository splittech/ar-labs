using UnityEngine;

namespace Game.Gameplay
{
    public interface IPudgeGestureEditorView
    {
        LayerMask PudgeLayerMask { get; }
        float MaxSwipeRotationAngle { get; }
    }
}
