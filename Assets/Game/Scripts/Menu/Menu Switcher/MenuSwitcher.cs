using R3;

namespace Game.Menu
{
    public class MenuSwitcher
    {
        private readonly MenuSwitcherView _menuSwitcherView;

        private MenuView _currentMenuView;

        public Observable<MenuView> OnMenuSwitched;

        public MenuSwitcher(MenuSwitcherView menuSwitcherView)
        {
            _menuSwitcherView = menuSwitcherView;
        }

        public void Initialize()
        {
            OnMenuSwitched = _menuSwitcherView.OnSwitchMenuButtonClicked;

            _menuSwitcherView.OnSwitchMenuButtonClicked
                .Subscribe(SwitchMenuView);
        }

        public void SwitchMenuView(MenuView menuView)
        {
            if (_currentMenuView != null)
                _currentMenuView.Hide();

            menuView.Show();

            _currentMenuView = menuView;
        }
    }
}
