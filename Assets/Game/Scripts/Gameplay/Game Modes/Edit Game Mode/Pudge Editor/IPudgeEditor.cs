using R3;

namespace Game.Gameplay
{
    public interface IPudgeEditor
    {
        ReadOnlyReactiveProperty<Pudge> SelectedPudge { get; }
        ReadOnlyReactiveProperty<Pudge> PreviousSelectedPudge { get; }
        ReadOnlyReactiveProperty<float> TotalScaleDelta { get; }
        ReadOnlyReactiveProperty<float> TotalAngleDelta { get; }

        void Enable();
        void Disable();
        void AddScale();
        void SubstractScale();
        void RotateClockwise();
        void RotateCounterClockwise();
        void ResetScaleAndRotation();
    }
}
