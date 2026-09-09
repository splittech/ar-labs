using UnityEngine;

namespace Game.Gameplay
{
    public class EmptyGameMode : GameMode
    {
        public override void Enable()
        {
            Debug.Log("EmptyGameMode: enabled");
        }

        public override void Disable()
        {
            Debug.Log("EmptyGameMode: disabled");
        }
    }
}