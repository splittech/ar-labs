using Game.Menu;
using R3;

namespace Game.Gameplay
{
    public class GameModeSwitcher
    {
        private readonly MenuSwitcher _menuSwitcher;
        private readonly GameModeResolver _gameModeResolver;

        private GameMode _currentGameMode;

        public GameModeSwitcher(
            MenuSwitcher menuSwitcher,
            GameModeResolver gameModeResolver)
        {
            _menuSwitcher = menuSwitcher;
            _gameModeResolver = gameModeResolver;
        }

        public void Initialize()
        {
            _menuSwitcher.OnMenuSwitched.Subscribe(SwitchGameMode);
        }

        private void SwitchGameMode(MenuView menuView)
        {
            _currentGameMode?.Disable();

            _currentGameMode = _gameModeResolver.GetGameMode(menuView);

            _currentGameMode.Enable();
        }
    }
}
