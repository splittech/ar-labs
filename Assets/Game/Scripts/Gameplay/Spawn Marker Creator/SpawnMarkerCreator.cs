using Game.Core.AR;
using Game.Core.Input;
using R3;
using UnityEngine;

namespace Game.Gameplay
{
    public class SpawnMarkerCreator
    {
        private readonly SpawnMarkerCreatorView _spawnMarkerCreatorView;
        private readonly InputService _inputService;
        private readonly ARRaycastService _raycastService;

        private SpawnMarker _currentSpawnMarker;
        private DisposableBag _disposableBag;
        private bool _enabled;

        public SpawnMarkerCreator(SpawnMarkerCreatorView spawnMarkerCreatorView, InputService inputService, ARRaycastService raycastService)
        {
            _spawnMarkerCreatorView = spawnMarkerCreatorView;
            _inputService = inputService;
            _raycastService = raycastService;
        }

        public SpawnMarker CurrentSpawnMarker => _currentSpawnMarker;

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
                .Subscribe(_ => DeleteMarker())
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

            SpawnMarkerView spawnMarkerView = _spawnMarkerCreatorView.CreateSpawnMarkerObject(pose.position, pose.rotation);
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

        private void DeleteMarker()
        {
            if (_currentSpawnMarker == null)
                return;

            _currentSpawnMarker.Delete();
            _currentSpawnMarker = null;
        }
    }
}