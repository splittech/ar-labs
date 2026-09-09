using VContainer.Unity;

namespace Game.Menu
{
    public class MenuBootstrap : IStartable
    {
        private readonly MenuSwitcher _menuSwitcher;
        private readonly MenuView _initialMenuView;

        public MenuBootstrap(MenuSwitcher menuSwitcher, MenuView initialMenuView)
        {
            _menuSwitcher = menuSwitcher;
            _initialMenuView = initialMenuView;
        }

        public void Start()
        {
            _menuSwitcher.Initialize();
            _menuSwitcher.SwitchMenuView(_initialMenuView);
        }
    }
}
