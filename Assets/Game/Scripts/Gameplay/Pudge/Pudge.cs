using System;
using Cysharp.Threading.Tasks;
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

        private readonly IPudgeView _view;
        private readonly ITickService _tickService;

        private readonly ReactiveProperty<Vector3?> _targetPosition = new(null);
        private readonly ReactiveProperty<float?> _remainingRotationAngle = new(null);
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
        public bool Disposed => _disposed;
        public bool IsDespawning => _isDespawning;
        public bool IsValid => _initialized && !_disposed && !_isDespawning;

        public ReadOnlyReactiveProperty<Vector3?> TargetPosition => _targetPosition;
        public ReadOnlyReactiveProperty<float?> RemainingRotationAngle => _remainingRotationAngle;
        public ReadOnlyReactiveProperty<float?> TargetScale => _targetScale;

        public Pudge(IPudgeView pudgeView, ITickService tickService)
        {
            _view = pudgeView;
            _tickService = tickService;
        }

        public void Initialize(
            Pose? initialPose = null,
            State initialState = State.Normal,
            float initialScale = 1f)
        {
            CheckIsNotDisposed();
            CheckIsNotDespawning();
            if (_initialized)
                return;

            initialPose ??= Pose.identity;

            _view.Initialize(this);

            SetPose(initialPose.Value);
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
            if (!_initialized)
            {
                Dispose();
                return;
            }

            if (_isDespawning || _disposed)
                return;

            ScaleTo(0f, EasingType.Linear);
            _view.ChangeAlphaTo(0f);

            _isDespawning = true;

            _targetScale
                .Where(value => value == null)
                .Take(1)
                .Subscribe(_ => Dispose())
                .AddTo(ref _disposableBag);
        }

        public void MoveTo(Vector3 targetPosition, EasingType easingType = EasingType.Instant, float speedMultiplier = 1f)
        {
            CheckIsValid();

            _movementEasingType = easingType;

            if (easingType == EasingType.Instant)
            {
                _targetPosition.Value = null;
                SetPosition(targetPosition);
                return;
            }

            _movementSpeed = _view.LinearMovementSpeed * speedMultiplier;
            _targetPosition.Value = targetPosition;
        }

        public void RotateBy(float angle, EasingType easingType, float speedMultiplier = 1f)
        {
            CheckIsValid();

            StartRotation((_remainingRotationAngle.Value ?? 0f) + angle, easingType, speedMultiplier);
        }

        public void RotateTo(Vector3 targetPosition, EasingType easingType, float speedMultiplier = 1f)
        {
            CheckIsValid();

            Vector3 lookDirection = targetPosition - _currentPose.position;
            lookDirection.y = 0f;

            if (lookDirection.magnitude < 0.001f)
                return;

            RotateTo(Quaternion.LookRotation(lookDirection, Vector3.up), easingType, speedMultiplier);
        }

        public void RotateTo(Quaternion targetRotation, EasingType easingType = EasingType.Instant, float speedMultiplier = 1f)
        {
            CheckIsValid();

            float currentYaw = _currentPose.rotation.eulerAngles.y;
            float targetYaw = targetRotation.eulerAngles.y;

            StartRotation(Mathf.DeltaAngle(currentYaw, targetYaw), easingType, speedMultiplier);
        }

        public void ScaleTo(float targetScale, EasingType easingType = EasingType.Instant, float speedMultiplier = 1f)
        {
            CheckIsValid();

            _scalingEasingType = easingType;

            if (easingType == EasingType.Instant)
            {
                _targetScale.Value = null;
                SetScale(targetScale);
                return;
            }

            _scalingSpeed = _view.LinearScaleSpeed * speedMultiplier;
            _targetScale.Value = targetScale;
        }

        public bool IsTransforming()
        {
            return
                TargetPosition.CurrentValue != null ||
                RemainingRotationAngle.CurrentValue != null ||
                TargetScale.CurrentValue != null;
        }

        public void Select()
        {
            CheckIsValid();

            _selected = true;
            _view.Select();
        }

        public void Deselect()
        {
            CheckIsValid();

            _selected = false;
            _view.Deselect();
        }

        private void SetState(State state)
        {
            AnimatorState animatorState = state switch
            {
                State.Normal => AnimatorState.Normal,
                State.Happy => AnimatorState.Happy,
                State.Sad => AnimatorState.Sad,
                _ => throw new NotImplementedException()
            };

            _view.SetAnimatorState(animatorState);
            _currentState = state;
        }

        private void StartRotation(float angle, EasingType easingType, float speedMultiplier)
        {
            _rotationEasingType = easingType;

            if (easingType == EasingType.Instant)
            {
                _remainingRotationAngle.Value = null;
                SetRotation(Quaternion.AngleAxis(angle, Vector3.up) * _currentPose.rotation);
                return;
            }

            _rotationSpeed = _view.LinearRotationSpeed * speedMultiplier;
            _remainingRotationAngle.Value = angle;
        }

        private void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            _disposableBag.Dispose();
            _targetPosition.Dispose();
            _remainingRotationAngle.Dispose();
            _targetScale.Dispose();
            _view.DestroyObject();
        }

        private void OnUpdate(Tick tick)
        {
            UpdatePosition(tick.DeltaTime);
            UpdateRotation(tick.DeltaTime);
            UpdateScale(tick.DeltaTime); // последним: по окончании Despawn здесь вызывается Dispose
        }

        private void UpdatePosition(float deltaTime)
        {
            if (_targetPosition.Value is not Vector3 targetPosition)
                return;

            float speed = _movementSpeed;
            if (_movementEasingType == EasingType.Damped)
            {
                speed = Mathf.Max(
                    Vector3.Distance(_currentPose.position, targetPosition) * _view.DampedMovementSharpness,
                    _view.DampedMinimumMovementSpeed);
            }

            Vector3 newPosition = Vector3.MoveTowards(_currentPose.position, targetPosition, speed * deltaTime);
            bool reached = (newPosition - targetPosition).sqrMagnitude < 0.000001f;

            SetPosition(reached ? targetPosition : newPosition);

            if (reached)
                _targetPosition.Value = null;
        }

        private void UpdateRotation(float deltaTime)
        {
            if (_remainingRotationAngle.Value is not float remaining)
                return;

            float speed = _rotationSpeed;
            if (_rotationEasingType == EasingType.Damped)
            {
                speed = Mathf.Max(
                    Mathf.Abs(remaining) * _view.DampedRotationSharpness,
                    _view.DampedMinimumRotationSpeed);
            }

            // Шаг со знаком, по модулю не больше оставшегося угла.
            float step = Mathf.MoveTowards(0f, remaining, speed * deltaTime);
            SetRotation(Quaternion.AngleAxis(step, Vector3.up) * _currentPose.rotation);

            remaining -= step;
            _remainingRotationAngle.Value = Mathf.Abs(remaining) < 0.01f ? null : remaining;
        }

        private void UpdateScale(float deltaTime)
        {
            if (_targetScale.Value is not float targetScale)
                return;

            float speed = _scalingSpeed;
            if (_scalingEasingType == EasingType.Damped)
            {
                speed = Mathf.Max(
                    Mathf.Abs(targetScale - _currentScale) * _view.DampedScalingSharpness,
                    _view.DampedMinimumScalingSpeed);
            }

            float newScale = Mathf.MoveTowards(_currentScale, targetScale, speed * deltaTime);
            bool reached = Mathf.Abs(newScale - targetScale) < 0.0001f;

            SetScale(reached ? targetScale : newScale);

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

        private void CheckIsValid()
        {
            CheckIsNotDisposed();
            CheckIsNotDespawning();
            CheckInitialized();
        }

        private void CheckInitialized()
        {
            if (!_initialized)
                throw new InvalidOperationException($"{nameof(Pudge)} is not initialized.");
        }

        private void CheckIsNotDespawning()
        {
            if (_isDespawning)
                throw new InvalidOperationException($"{nameof(Pudge)} is despawning.");
        }

        private void CheckIsNotDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(Pudge));
        }
    }
}