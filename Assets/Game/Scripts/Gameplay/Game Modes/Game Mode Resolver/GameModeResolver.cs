using Game.Menu;

namespace Game.Gameplay
{
    public class GameModeResolver : IGameModeResolver
    {
        private readonly EmptyGameMode _emptyGameMode;

        private readonly CreateGameMode _createGameMode;
        private readonly CreateGameModeView _createGameModeView;

        private readonly EditGameMode _editGameMode;
        private readonly EditGameModeView _editGameModeView;

        private readonly ImageTrackingMode _imageTrackingMode;
        private readonly ImageTrackingModeView _imageTrackingModeView;

        private readonly FaceTrackingMode _faceTrackingMode;
        private readonly FaceTrackingModeView _faceTrackingModeView;

        public GameModeResolver(
            EmptyGameMode emptyGameMode,
            CreateGameMode createGameMode,
            CreateGameModeView createGameModeView,
            EditGameMode editGameMode,
            EditGameModeView editGameModeView,
            ImageTrackingMode imageTrackingMode,
            ImageTrackingModeView imageTrackingModeView,
            FaceTrackingMode faceTrackingMode,
            FaceTrackingModeView faceTrackingModeView)
        {
            _emptyGameMode = emptyGameMode;
            _createGameMode = createGameMode;
            _createGameModeView = createGameModeView;
            _editGameMode = editGameMode;
            _editGameModeView = editGameModeView;
            _imageTrackingMode = imageTrackingMode;
            _imageTrackingModeView = imageTrackingModeView;
            _faceTrackingMode = faceTrackingMode;
            _faceTrackingModeView = faceTrackingModeView;
        }

        public GameMode ResolveGameMode(IMenuView menuView)
        {
            if (menuView == _createGameModeView)
                return _createGameMode;

            if (menuView == _editGameModeView)
                return _editGameMode;

            if (menuView == _imageTrackingModeView)
                return _imageTrackingMode;

            if (menuView == _faceTrackingModeView)
                return _faceTrackingMode;

            return _emptyGameMode;
        }
    }
}