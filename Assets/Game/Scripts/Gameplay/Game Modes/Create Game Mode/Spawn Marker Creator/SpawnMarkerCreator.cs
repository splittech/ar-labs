using Game.Core.AR;
using Game.Core.Input;
using R3;
using UnityEngine;

namespace Game.Gameplay
{
    public class SpawnMarkerCreator : ISpawnMarkerCreator
    {
        private readonly ISpawnMarkerCreatorView _spawnMarkerCreatorView;
        private readonly IInputService _inputService;
        private readonly IRaycastService _raycastService;

        private SpawnMarker _currentSpawnMarker;
        private DisposableBag _disposableBag;
        private bool _enabled;

        private readonly Subject<Pose> _onSpawnMarkerReleased = new();

        public SpawnMarkerCreator(ISpawnMarkerCreatorView spawnMarkerCreatorView, IInputService inputService, IRaycastService raycastService)
        {
            _spawnMarkerCreatorView = spawnMarkerCreatorView;
            _inputService = inputService;
            _raycastService = raycastService;
        }

        public SpawnMarker CurrentSpawnMarker => _currentSpawnMarker;

        // Срабатывает, когда палец отпущен над маркером. Маркер к этому моменту уже удалён,
        // поэтому подписчикам передаётся его последняя поза.
        public Observable<Pose> OnSpawnMarkerReleased => _onSpawnMarkerReleased;

        public void Enable()
        {
            if (_enabled)
                return;

            _enabled = true;

            _inputService.OnInputActionPerformed
                .Where(context => context is
                {
                    ActionType: ActionType.Press,
                    ActionStatus: ActionStatus.Started,
                    IsOverUI: false
                })
                .Subscribe(CreateSpawnMarker)
                .AddTo(ref _disposableBag);

            _inputService.OnInputActionPerformed
                .Where(context => context is
                {
                    ActionType: ActionType.Drag,
                    ActionStatus: ActionStatus.Performed,
                })
                .Subscribe(MoveMarker)
                .AddTo(ref _disposableBag);

            _inputService.OnInputActionPerformed
                .Where(context => context is
                {
                    ActionType: ActionType.Press,
                    ActionStatus: ActionStatus.Canceled,
                })
                .Subscribe(_ => ReleaseMarker())
                .AddTo(ref _disposableBag);
        }

        public void Disable()
        {
            if (!_enabled)
                return;

            _enabled = false;
            _disposableBag.Clear();

            DeleteMarker();
        }

        private void CreateSpawnMarker(InputContext context)
        {
            if (!_raycastService.RaycastOnFloor(context.ScreenPosition, out Pose pose))
                return;

            ISpawnMarkerView spawnMarkerView = _spawnMarkerCreatorView.CreateSpawnMarkerObject(pose.position, pose.rotation);
            _currentSpawnMarker = new SpawnMarker(spawnMarkerView, pose);
        }

        private void MoveMarker(InputContext context)
        {
            if (_currentSpawnMarker == null)
                return;

            if (context.IsOverUI)
            {
                _currentSpawnMarker.Delete();
                _currentSpawnMarker = null;
                return;
            }

            if (!_raycastService.RaycastOnFloor(context.ScreenPosition, out Pose pose))
                return;

            _currentSpawnMarker.SetPositionAndRotation(pose.position, pose.rotation);
        }

        private void ReleaseMarker()
        {
            if (_currentSpawnMarker == null)
                return;

            Pose pose = _currentSpawnMarker.Pose;

            DeleteMarker();
            _onSpawnMarkerReleased.OnNext(pose);
        }

        private void DeleteMarker()
        {
            if (_currentSpawnMarker == null)
                return;

            _currentSpawnMarker.Delete();
            _currentSpawnMarker = null;
        }
    }
}