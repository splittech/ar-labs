using R3;
using UnityEngine;

namespace Game.Core
{
    public enum TickType
    {
        Update,
        FixedUpdate,
        LateUpdate
    }

    public readonly struct Tick
    {
        public readonly TickType Type;
        public readonly float DeltaTime;

        public Tick(TickType type, float deltaTime)
        {
            Type = type;
            DeltaTime = deltaTime;
        }
    }

    public class TickService : MonoBehaviour, ITickService
    {
        private Subject<Tick> _onTick = new();

        public Observable<Tick> OnTick => _onTick;

        private void Update()
        {
            _onTick.OnNext(new Tick(TickType.Update, Time.deltaTime));
        }

        private void LateUpdate()
        {
            _onTick.OnNext(new Tick(TickType.LateUpdate, Time.deltaTime));
        }

        private void FixedUpdate()
        {
            _onTick.OnNext(new Tick(TickType.FixedUpdate, Time.fixedDeltaTime));
        }
    }
}