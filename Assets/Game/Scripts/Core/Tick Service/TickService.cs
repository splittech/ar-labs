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

    public class TickService : MonoBehaviour
    {
        public struct Tick
        {
            public TickType Type;
            public float DeltaTime;
        }

        private Subject<Tick> _onTick = new();

        public Observable<Tick> OnTick => _onTick;

        private void Update()
        {
            _onTick.OnNext(new Tick() { Type = TickType.Update, DeltaTime = Time.deltaTime });
        }

        private void LateUpdate()
        {
            _onTick.OnNext(new Tick() { Type = TickType.LateUpdate, DeltaTime = Time.deltaTime });
        }

        private void FixedUpdate()
        {
            _onTick.OnNext(new Tick() { Type = TickType.FixedUpdate, DeltaTime = Time.fixedDeltaTime });
        }
    }
}