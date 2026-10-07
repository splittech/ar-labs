using R3;

namespace Game.Menu
{
    public interface IMenuSwitcher
    {
        ReadOnlyReactiveProperty<IMenuView> CurrentMenuView { get; }

        void Initialize();
        void SwitchMenuView(IMenuView menuView);
    }
}
