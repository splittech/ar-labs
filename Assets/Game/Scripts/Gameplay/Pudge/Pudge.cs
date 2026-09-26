using System;
using Game.Core;
using R3;
using UnityEngine;

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

        public enum EasingType
        {
            Instant,
            Linear,
            Damped,
        }

        private readonly PudgeView _view;
        private readonly TickService _tickService;

        private readonly ReactiveProperty<Vector3?> _targetPosition = new(null);
        private readonly ReactiveProperty<Quaternion?> _targetRotation = new(null);
        private readonly ReactiveProperty<float?> _targetScale = new(null);

        private EasingType _movementEasingType;
        private EasingType _rotationEasingType;
        private EasingType _scalingEasingType;

        private State _currentState;
        private Pose _currentPose;
        private float _currentScale;

        private float _movementSpeed;
        private float _rotationSpeed;
        private float _scalingSpeed;

        private bool _initialized;
        private bool _disposed;
        private bool _selected;
        private bool _isDespawning;

        private DisposableBag _disposableBag;

        public State CurrentState => _currentState;
        public Pose CurrentPose => _currentPose;
        public float CurrentScale => _currentScale;
        public string Name => _view.Name;
        public string Description => _view.Description;
        public bool Selected => _selected;
        public bool IsDespawning => _isDespawning;

        public ReadOnlyReactiveProperty<Vector3?> TargetPosition => _targetPosition;
        public ReadOnlyReactiveProperty<Quaternion?> TargetRotation => _targetRotation;
        public ReadOnlyReactiveProperty<float?> TargetScale => _targetScale;

        public Pudge(PudgeView pudgeView, TickService tickService)
        {
            _view = pudgeView;
            _tickService = tickService;
        }

        public void Initialize(
            Pose initialPose,
            State initialState,
            float initialScale)
        {
            CheckDisposed();
            CheckDespawning();

            if (_initialized)
                return;

            _view.Initialize(this);

            SetPose(initialPose);
            SetScale(initialScale);
            SetState(initialState);

            _tickService.OnTick
                .Where(tick => tick.Type == TickType.Update)
                .Subscribe(OnUpdate)
                .AddTo(ref _disposableBag);

            _initialized = true;
        }

        public void Despawn()
        {
            if (_isDespawning)
                return;
            _isDespawning = true;

            ScaleTo(0f, EasingType.Linear);
            TargetScale
                .Where(value => value == null)
                .Take(1)
                .Subscribe(_ => Dispose());
        }

        public void MoveTo(Vector3 targetPosition, EasingType easingType, float speedMultiplier = 1f)
        {
            CheckDisposed();
            CheckDespawning();

            if (easingType == EasingType.Instant)
            {
                SetPosition(targetPosition);
            }
            else if (easingType == EasingType.Linear)
            {
                _targetPosition.Value = targetPosition;
                _movementSpeed = _view.LinearMovementSpeed * speedMultiplier;
            }
            else if (easingType == EasingType.Damped)
            {
                _targetPosition.Value = targetPosition;
                _movementSpeed = _view.DampedInitialMovementSpeed * speedMultiplier;
            }
        }

        public void RotateTo(Vector3 targetPosition, EasingType easingType, float speedMultiplier = 1f)
        {
            Vector3 lookDirection = targetPosition - _currentPose.position;
            lookDirection.y = 0f;
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);

            RotateTo(targetRotation, easingType, speedMultiplier);
        }

        public void RotateTo(Quaternion targetRotation, EasingType easingType, float speedMultiplier = 1f)
        {
            CheckDisposed();
            CheckDespawning();

            if (easingType == EasingType.Instant)
            {
                SetRotation(targetRotation);
            }
            else if (easingType == EasingType.Linear)
            {
                _targetRotation.Value = targetRotation;
                _movementSpeed = _view.LinearMovementSpeed * speedMultiplier;
            }
            else if (easingType == EasingType.Damped)
            {
                _targetRotation.Value = targetRotation;
                _movementSpeed = _view.DampedInitialMovementSpeed * speedMultiplier;
            }
        }

        public void ScaleTo(float targetScale, EasingType easingType, float speedMultiplier = 1f)
        {
            CheckDisposed();
            CheckDespawning();

            if (easingType == EasingType.Instant)
            {
                SetScale(targetScale);
            }
            else if (easingType == EasingType.Linear)
            {
                _targetScale.Value = targetScale;
                _movementSpeed = _view.LinearMovementSpeed * speedMultiplier;
            }
            else if (easingType == EasingType.Damped)
            {
                _targetScale.Value = targetScale;
                _movementSpeed = _view.DampedInitialMovementSpeed * speedMultiplier;
            }
        }

        public bool IsTransforming()
        {
            return
                TargetPosition.CurrentValue != null ||
                TargetRotation.CurrentValue != null ||
                TargetScale.CurrentValue != null;
        }

        public void SetState(State state)
        {
            CheckDisposed();
            CheckDespawning();

            PudgeView.AnimatorState animatorState = state switch
            {
                State.Normal => PudgeView.AnimatorState.Normal,
                State.Happy => PudgeView.AnimatorState.Happy,
                State.Sad => PudgeView.AnimatorState.Sad,
                _ => throw new ArgumentOutOfRangeException()
            };

            _view.SetAnimatorState(animatorState);
            _currentState = state;
        }

        public void Select()
        {
            CheckDisposed();
            CheckDespawning();

            _selected = true;
            _view.Select();
        }

        public void Deselect()
        {
            CheckDisposed();
            CheckDespawning();

            _selected = false;
            _view.Deselect();
        }

        private void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            _disposableBag.Dispose();
            _targetPosition.Dispose();
            _targetRotation.Dispose();
            _view.DestroyObject();
        }

        private void OnUpdate(TickService.Tick tick)
        {
            UpdatePosition(tick.DeltaTime);
            UpdateRotation(tick.DeltaTime);
            UpdateScale(tick.DeltaTime);
        }

        private void UpdatePosition(float deltaTime)
        {
            if (_disposed)
                return;

            if (_targetPosition.Value is not Vector3 targetPosition)
                return;

            Vector3 newPosition = Vector3.MoveTowards(
                _currentPose.position,
                targetPosition,
                _movementSpeed * deltaTime);

            bool reached = (newPosition - targetPosition).magnitude < 0.001f;

            SetPosition(reached ? targetPosition : newPosition);

            if (_movementEasingType == EasingType.Damped)
                _movementSpeed *= _view.DampedMovementSpeedLoss;

            if (reached)
                _targetPosition.Value = null;
        }

        private void UpdateRotation(float deltaTime)
        {
            if (_disposed)
                return;

            if (_targetRotation.Value is not Quaternion targetRotation)
                return;

            Quaternion newRotation = Quaternion.RotateTowards(
                _currentPose.rotation,
                targetRotation,
                _rotationSpeed * deltaTime);

            bool reached = Quaternion.Angle(newRotation, targetRotation) < 0.1f;

            SetRotation(reached ? targetRotation : newRotation);

            if (_rotationEasingType == EasingType.Damped)
                _rotationSpeed *= _view.DampedRotationSpeedLoss;

            if (reached)
                _targetRotation.Value = null;
        }

        private void UpdateScale(float deltaTime)
        {
            if (_disposed)
                return;

            if (_targetScale.Value is not float targetScale)
                return;

            float newScale = _currentScale + Mathf.Sign(targetScale - _currentScale) * _scalingSpeed * deltaTime;
            bool reached = Math.Abs(targetScale - _currentScale) < 0.01f;

            SetScale(reached ? targetScale : newScale);

            if (_scalingEasingType == EasingType.Damped)
                _scalingSpeed *= _view.DampedScalingSpeedLoss;

            if (reached)
                _targetScale.Value = null;
        }

        private void SetPose(Pose pose)
        {
            _currentPose = pose;
            SetPosition(_currentPose.position);
            SetRotation(_currentPose.rotation);
        }

        private void SetPosition(Vector3 position)
        {
            _currentPose.position = position;
            _view.SetPosition(position);
        }

        private void SetRotation(Quaternion rotation)
        {
            _currentPose.rotation = rotation;
            _view.SetRotation(rotation);
        }

        private void SetScale(float scale)
        {
            _currentScale = scale;
            _view.SetScale(scale);
        }

        private void CheckDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Pudge));
        }

        private void CheckDespawning()
        {
            if (_isDespawning)
                throw new InvalidOperationException(nameof(Pudge) + "is dispawning.");
        }
    }
}
