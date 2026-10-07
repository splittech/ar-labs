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
            builder.Register<MenuSwitcher>(Lifetime.Singleton).AsSelf().As<IMenuSwitcher>();
            builder.RegisterComponent(_menuSwitcherView).As<IMenuSwitcherView>();
            builder.RegisterComponent(_initialMenuView);
        }
    }
}
