using Game.Gameplay;
using Game.Menu;
using NSubstitute;
using NUnit.Framework;

namespace Game.Tests.EditMode
{
    public class GameModeSwitcherTests
    {
        [Test]
        public void Initialize_MenuIsAlreadySwitched_GameModeOfCurrentMenuIsEnabled()
        {
            // Arrange.
            GameModeSwitcher gameModeSwitcher = Setup.GameModeSwitcher(out var currentMenuView, out var gameModeResolver);

            var menuView = Substitute.For<IMenuView>();
            var gameMode = Substitute.For<GameMode>();
            gameModeResolver.ResolveGameMode(menuView).Returns(gameMode);

            currentMenuView.Value = menuView;

            // Act.
            gameModeSwitcher.Initialize();

            // Assert.
            gameMode.Received(1).Enable();
        }

        [Test]
        public void Initialize_NoCurrentMenu_GameModeIsNotResolved()
        {
            // Arrange.
            GameModeSwitcher gameModeSwitcher = Setup.GameModeSwitcher(out var _, out var gameModeResolver);

            // Act.
            gameModeSwitcher.Initialize();

            // Assert.
            gameModeResolver.DidNotReceive().ResolveGameMode(Arg.Any<IMenuView>());
        }

        [Test]
        public void MenuSwitched_GameModeSwitcherIsInitialized_GameModeOfNewMenuIsEnabled()
        {
            // Arrange.
            GameModeSwitcher gameModeSwitcher = Setup.GameModeSwitcher(out var currentMenuView, out var gameModeResolver);

            var menuView = Substitute.For<IMenuView>();
            var gameMode = Substitute.For<GameMode>();
            gameModeResolver.ResolveGameMode(menuView).Returns(gameMode);

            gameModeSwitcher.Initialize();

            // Act.
            currentMenuView.Value = menuView;

            // Assert.
            gameMode.Received(1).Enable();
        }

        [Test]
        public void MenuSwitched_OtherGameModeWasEnabled_PreviousGameModeIsDisabled()
        {
            // Arrange.
            GameModeSwitcher gameModeSwitcher = Setup.GameModeSwitcher(out var currentMenuView, out var gameModeResolver);

            var previousMenuView = Substitute.For<IMenuView>();
            var previousGameMode = Substitute.For<GameMode>();
            gameModeResolver.ResolveGameMode(previousMenuView).Returns(previousGameMode);

            var nextMenuView = Substitute.For<IMenuView>();
            var nextGameMode = Substitute.For<GameMode>();
            gameModeResolver.ResolveGameMode(nextMenuView).Returns(nextGameMode);

            gameModeSwitcher.Initialize();
            currentMenuView.Value = previousMenuView;

            // Act.
            currentMenuView.Value = nextMenuView;

            // Assert.
            previousGameMode.Received(1).Disable();
        }

        [Test]
        public void Initialize_CalledTwice_GameModeIsEnabledOnce()
        {
            // Arrange.
            GameModeSwitcher gameModeSwitcher = Setup.GameModeSwitcher(out var currentMenuView, out var gameModeResolver);

            var menuView = Substitute.For<IMenuView>();
            var gameMode = Substitute.For<GameMode>();
            gameModeResolver.ResolveGameMode(menuView).Returns(gameMode);

            currentMenuView.Value = menuView;

            // Act.
            gameModeSwitcher.Initialize();
            gameModeSwitcher.Initialize();

            // Assert.
            gameMode.Received(1).Enable();
        }
    }
}
