using UnityEngine;

namespace Game.Gameplay
{
    public class CreateGameMode : GameMode
    {
        public override void Enable()
        {
            Debug.Log("CreateGameMode: enabled");
        }

        public override void Disable()
        {
            Debug.Log("CreateGameMode: disabled");
        }
    }
}
