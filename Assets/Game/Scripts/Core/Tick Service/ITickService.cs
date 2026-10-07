using R3;

namespace Game.Core
{
    public interface ITickService
    {
        Observable<Tick> OnTick { get; }
    }
}