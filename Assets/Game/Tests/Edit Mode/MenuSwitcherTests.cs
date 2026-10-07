using Game.Menu;
using NSubstitute;
using NUnit.Framework;

namespace Game.Tests.EditMode
{
    public class MenuSwitcherTests
    {
        [Test]
        public void SwitchMenuView_NoCurrentMenu_MenuIsShown()
        {
            // Arrange.
            MenuSwitcher menuSwitcher = Setup.MenuSwitcher();
            var menuView = Substitute.For<IMenuView>();

            // Act.
            menuSwitcher.SwitchMenuView(menuView);

            // Assert.
            menuView.Received(1).Show();
        }

        [Test]
        public void SwitchMenuView_OtherMenu_PreviousMenuIsHidden()
        {
            // Arrange.
            MenuSwitcher menuSwitcher = Setup.MenuSwitcher();
            var previousMenuView = Substitute.For<IMenuView>();
            var nextMenuView = Substitute.For<IMenuView>();
            menuSwitcher.SwitchMenuView(previousMenuView);

            // Act.
            menuSwitcher.SwitchMenuView(nextMenuView);

            // Assert.
            previousMenuView.Received(1).Hide();
        }

        [Test]
        public void SwitchMenuView_OtherMenu_CurrentMenuViewIsOtherMenu()
        {
            // Arrange.
            MenuSwitcher menuSwitcher = Setup.MenuSwitcher();
            var previousMenuView = Substitute.For<IMenuView>();
            var nextMenuView = Substitute.For<IMenuView>();
            menuSwitcher.SwitchMenuView(previousMenuView);

            // Act.
            menuSwitcher.SwitchMenuView(nextMenuView);

            // Assert.
            Assert.That(menuSwitcher.CurrentMenuView.CurrentValue, Is.SameAs(nextMenuView));
        }

        [Test]
        public void SwitchMenuView_SameMenu_MenuIsNotHidden()
        {
            // Arrange.
            MenuSwitcher menuSwitcher = Setup.MenuSwitcher();
            var menuView = Substitute.For<IMenuView>();
            menuSwitcher.SwitchMenuView(menuView);

            // Act.
            menuSwitcher.SwitchMenuView(menuView);

            // Assert.
            menuView.DidNotReceive().Hide();
        }

        [Test]
        public void SwitchMenuButtonClicked_MenuSwitcherIsInitialized_CurrentMenuViewIsClickedMenu()
        {
            // Arrange.
            MenuSwitcher menuSwitcher = Setup.MenuSwitcher(out var onSwitchMenuButtonClicked);
            var menuView = Substitute.For<IMenuView>();
            menuSwitcher.Initialize();

            // Act.
            onSwitchMenuButtonClicked.OnNext(menuView);

            // Assert.
            Assert.That(menuSwitcher.CurrentMenuView.CurrentValue, Is.SameAs(menuView));
        }

        [Test]
        public void SwitchMenuButtonClicked_MenuSwitcherIsNotInitialized_CurrentMenuViewIsNull()
        {
            // Arrange.
            MenuSwitcher menuSwitcher = Setup.MenuSwitcher(out var onSwitchMenuButtonClicked);
            var menuView = Substitute.For<IMenuView>();

            // Act.
            onSwitchMenuButtonClicked.OnNext(menuView);

            // Assert.
            Assert.That(menuSwitcher.CurrentMenuView.CurrentValue, Is.Null);
        }

        [Test]
        public void Initialize_CalledTwice_ClickedMenuIsShownOnce()
        {
            // Arrange.
            MenuSwitcher menuSwitcher = Setup.MenuSwitcher(out var onSwitchMenuButtonClicked);
            var menuView = Substitute.For<IMenuView>();

            // Act.
            menuSwitcher.Initialize();
            menuSwitcher.Initialize();
            onSwitchMenuButtonClicked.OnNext(menuView);

            // Assert.
            menuView.Received(1).Show();
        }
    }
}
