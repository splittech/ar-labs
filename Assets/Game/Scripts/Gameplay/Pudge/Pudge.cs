using System;
using Game.Core;
using R3;
using UnityEngine;

namespace Game.Gameplay
{
    public class Pudge : IDisposable
    {
        public enum State
        {
            None,
            Normal,
            Happy,
            Sad
        }

        private readonly PudgeView _pudgeView;
        private readonly TickService _tickService;

        private readonly ReactiveProperty<Vector3?> _targetPosition = new(null);
        private readonly ReactiveProperty<Quaternion?> _targetRotation = new(null);

        private State _currentState;
        private Pose _currentPose;
        private float _currentScale;

        private float _movementSpeed;
        private float _rotationSpeed;

        private bool _initialized;
        private bool _disposed;
        private bool _selected;

        private DisposableBag _disposableBag;

        public State CurrentState => _currentState;
        public Pose CurrentPose => _currentPose;
        public float CurrentScale => _currentScale;
        public string Name => _pudgeView.Name;
        public string Description => _pudgeView.Description;
        public bool Selected => _selected;

        public ReadOnlyReactiveProperty<Vector3?> TargetPosition => _targetPosition;
        public ReadOnlyReactiveProperty<Quaternion?> TargetRotation => _targetRotation;

        public Pudge(PudgeView pudgeView, TickService tickService)
        {
            _pudgeView = pudgeView;
            _tickService = tickService;
        }

        public void Initialize(
            Pose initialPose,
            State initialState,
            float initialScale)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Pudge));

            if (_initialized)
                return;

            _pudgeView.Initialize(this);

            _movementSpeed = _pudgeView.MovementSpeed;
            _rotationSpeed = _pudgeView.RotationSpeed;

            SetPose(initialPose);
            SetScale(initialScale);
            SetState(initialState);

            _tickService.OnTick
                .Where(tick => tick.Type == TickType.Update)
                .Subscribe(OnUpdate)
                .AddTo(ref _disposableBag);

            _initialized = true;
        }

        public void SetTargetPosition(Vector3 position)
        {
            _targetPosition.Value = position;
        }

        public void RotateTowards(Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - _currentPose.position;
            direction.y = 0f;

            if (direction.magnitude < 0.001f)
            {
                _targetRotation.Value = null;
                return;
            }

            SetTargetRotation(Quaternion.LookRotation(direction, Vector3.up));
        }

        public void SetTargetRotation(Quaternion rotation)
        {
            _targetRotation.Value = rotation;
        }

        public void SetScale(float scale)
        {
            _currentScale = scale;
            _pudgeView.SetScale(scale);
        }

        public void SetState(State state)
        {
            PudgeView.AnimatorState animatorState = state switch
            {
                State.Normal => PudgeView.AnimatorState.Normal,
                State.Happy => PudgeView.AnimatorState.Happy,
                State.Sad => PudgeView.AnimatorState.Sad,
                _ => throw new ArgumentOutOfRangeException()
            };

            _pudgeView.SetAnimatorState(animatorState);
            _currentState = state;
        }

        public void Select()
        {
            _selected = true;
            _pudgeView.Select();
        }

        public void Deselect()
        {
            _selected = false;
            _pudgeView.Deselect();
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            _disposableBag.Dispose();
            _targetPosition.Dispose();
            _targetRotation.Dispose();
            _pudgeView.DestroyObject();
        }

        private void OnUpdate(TickService.Tick tick)
        {
            if (_disposed)
                return;

            UpdatePosition(tick.DeltaTime);

            if (_disposed)
                return;

            UpdateRotation(tick.DeltaTime);
        }

        private void UpdatePosition(float deltaTime)
        {
            if (_targetPosition.Value is not Vector3 targetPosition)
                return;

            Vector3 newPosition = Vector3.MoveTowards(
                _currentPose.position,
                targetPosition,
                _movementSpeed * deltaTime);

            bool reached = (newPosition - targetPosition).magnitude < 0.001f;

            SetPosition(reached ? targetPosition : newPosition);

            if (reached)
                _targetPosition.Value = null;
        }

        private void UpdateRotation(float deltaTime)
        {
            if (_targetRotation.Value is not Quaternion targetRotation)
                return;

            Quaternion newRotation = Quaternion.RotateTowards(
                _currentPose.rotation,
                targetRotation,
                _rotationSpeed * deltaTime);

            bool reached = Quaternion.Angle(newRotation, targetRotation) < 0.1f;

            SetRotation(reached ? targetRotation : newRotation);

            if (reached)
                _targetRotation.Value = null;
        }


        private void SetPosition(Vector3 position)
        {
            _currentPose.position = position;
            _pudgeView.SetPosition(position);
        }

        private void SetRotation(Quaternion rotation)
        {
            _currentPose.rotation = rotation;
            _pudgeView.SetRotation(rotation);
        }

        private void SetPose(Pose pose)
        {
            _currentPose = pose;
            SetPosition(_currentPose.position);
            SetRotation(_currentPose.rotation);
        }
    }
}
