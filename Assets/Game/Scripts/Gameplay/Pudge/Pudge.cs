using System;

namespace Game.Gameplay
{
    public class Pudge
    {
        public enum State
        {
            None,
            Normal,
            Happy,
            Sad
        }

        private readonly PudgeView _pudgeView;

        private State _currentState;

        public State CurrentState => _currentState;

        public Pudge(PudgeView pudgeView)
        {
            _pudgeView = pudgeView;
        }

        public void SetState(State state)
        {
            _currentState = state;

            PudgeView.AnimatorState animatorState = _currentState switch
            {
                State.Normal => PudgeView.AnimatorState.Normal,
                State.Happy => PudgeView.AnimatorState.Happy,
                State.Sad => PudgeView.AnimatorState.Sad,
                _ => throw new NotImplementedException()
            };

            _pudgeView.SetAnimatorState(animatorState);
        }

        public void Despawn()
        {
            _pudgeView.DestroyObject();
        }
    }
}
