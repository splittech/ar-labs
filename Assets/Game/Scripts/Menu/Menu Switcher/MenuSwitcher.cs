using R3;

namespace Game.Menu
{
    public class MenuSwitcher : IMenuSwitcher
    {
        private readonly IMenuSwitcherView _menuSwitcherView;

        private readonly ReactiveProperty<IMenuView> _currentMenuView = new();
        private bool _initialized;

        private DisposableBag _disposableBag;

        // Хранит текущее меню, поэтому подписчик сразу получает его значение
        // и не зависит от того, успел ли MenuSwitcher переключиться до подписки.
        public ReadOnlyReactiveProperty<IMenuView> CurrentMenuView => _currentMenuView;

        public MenuSwitcher(IMenuSwitcherView menuSwitcherView)
        {
            _menuSwitcherView = menuSwitcherView;
        }

        public void Initialize()
        {
            if (_initialized)
                return;
            _initialized = true;

            _menuSwitcherView.OnSwitchMenuButtonClicked
                .Subscribe(SwitchMenuView)
                .AddTo(ref _disposableBag);
        }

        public void SwitchMenuView(IMenuView menuView)
        {
            if (menuView == _currentMenuView.Value)
                return;

            _currentMenuView.Value?.Hide();
            menuView.Show();

            _currentMenuView.Value = menuView;
        }
    }
}
