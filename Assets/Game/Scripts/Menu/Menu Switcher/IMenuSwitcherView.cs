using R3;

namespace Game.Menu
{
    public interface IMenuSwitcherView
    {
        Observable<IMenuView> OnSwitchMenuButtonClicked { get; }
    }
}
