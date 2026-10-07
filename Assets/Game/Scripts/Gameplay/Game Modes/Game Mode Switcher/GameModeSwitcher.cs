using Game.Menu;
using R3;

namespace Game.Gameplay
{
    public class GameModeSwitcher
    {
        private readonly IMenuSwitcher _menuSwitcher;
        private readonly IGameModeResolver _gameModeResolver;

        private GameMode _currentGameMode;
        private bool _initialized;

        private DisposableBag _disposableBag;

        public GameModeSwitcher(
            IMenuSwitcher menuSwitcher,
            IGameModeResolver gameModeResolver)
        {
            _menuSwitcher = menuSwitcher;
            _gameModeResolver = gameModeResolver;
        }

        public void Initialize()
        {
            if (_initialized)
                return;
            _initialized = true;

            _menuSwitcher.CurrentMenuView
                .Where(menuView => menuView != null)
                .Subscribe(SwitchGameMode)
                .AddTo(ref _disposableBag);
        }

        private void SwitchGameMode(IMenuView menuView)
        {
            _currentGameMode?.Disable();

            _currentGameMode = _gameModeResolver.ResolveGameMode(menuView);

            _currentGameMode.Enable();
        }
    }
}
