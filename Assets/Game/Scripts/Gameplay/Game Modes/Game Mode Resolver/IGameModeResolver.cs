using Game.Menu;

namespace Game.Gameplay
{
    public interface IGameModeResolver
    {
        GameMode ResolveGameMode(IMenuView menuView);
    }
}
