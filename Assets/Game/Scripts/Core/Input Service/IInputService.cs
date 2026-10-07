using R3;

namespace Game.Core.Input
{
    public interface IInputService
    {
        Observable<InputContext> OnInputActionPerformed { get; }

        void Enable();
        void Disable();
    }
}
