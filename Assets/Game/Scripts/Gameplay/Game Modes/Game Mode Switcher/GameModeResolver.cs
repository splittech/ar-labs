using Game.Menu;

namespace Game.Gameplay
{
    public class GameModeResolver
    {
        private readonly CreateGameMode _createGameMode;
        private readonly CreateGameModeView _createGameModeView;
        private readonly EditGameMode _editGameMode;
        private readonly EditGameModeView _editGameModeView;
        private readonly EmptyGameMode _emptyGameMode;

        public GameModeResolver(
            CreateGameMode createGameMode,
            CreateGameModeView createGameModeView,
            EditGameMode editGameMode,
            EditGameModeView editGameModeView,
            EmptyGameMode emptyGameMode)
        {
            _createGameMode = createGameMode;
            _editGameMode = editGameMode;
            _createGameModeView = createGameModeView;
            _editGameModeView = editGameModeView;
            _emptyGameMode = emptyGameMode;
        }

        public GameMode GetGameMode(MenuView menuView)
        {
            if (menuView == _createGameModeView)
                return _createGameMode;

            if (menuView == _editGameModeView)
                return _editGameMode;

            return _emptyGameMode;
        }
    }
}