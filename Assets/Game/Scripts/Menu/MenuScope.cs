using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Game.Menu
{
    public class MenuScope : LifetimeScope
    {
        [Header("Menu")]
        [SerializeField] private MenuSwitcherView _menuSwitcherView;
        [SerializeField] private MenuView _initialMenuView;

        protected override void Configure(IContainerBuilder builder)
        {
            // Bootstrap.
            builder.RegisterEntryPoint<MenuBootstrap>();

            // Menu Switcher.
            builder.Register<MenuSwitcher>(Lifetime.Singleton);
            builder.RegisterComponent(_menuSwitcherView);
            builder.RegisterComponent(_initialMenuView);
        }
    }
}
