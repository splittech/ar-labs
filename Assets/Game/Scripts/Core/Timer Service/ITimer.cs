using R3;

namespace Game.Core
{
    public interface ITimer
    {
        ReadOnlyReactiveProperty<bool> Elapsed { get; }

        void Reset(float time);
        void Stop();
    }
}
