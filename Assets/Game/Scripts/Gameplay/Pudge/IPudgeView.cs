using R3;
using UnityEngine;

namespace Game.Gameplay
{
    public interface IPudgeView
    {
        Pudge Pudge { get; }
        string Name { get; }
        string Description { get; }
        float LinearMovementSpeed { get; }
        float DampedInitialMovementSpeed { get; }
        float DampedMovementSharpness { get; }
        float DampedMinimumMovementSpeed { get; }
        float LinearRotationSpeed { get; }
        float DampedInitialRotationSpeed { get; }
        float DampedRotationSharpness { get; }
        float DampedMinimumRotationSpeed { get; }
        float LinearScaleSpeed { get; }
        float DampedInitialScalingSpeed { get; }
        float DampedScalingSharpness { get; }
        float DampedMinimumScalingSpeed { get; }
        Observable<Unit> OnSelected { get; }

        void ChangeAlphaTo(float targetAlpha);
        void Deselect();
        void DestroyObject();
        void Initialize(Pudge pudge);
        void Select();
        void SetAnimatorState(AnimatorState animatorState);
        void SetPosition(Vector3 position);
        void SetRotation(Quaternion rotation);
        void SetScale(float scale);
    }
}