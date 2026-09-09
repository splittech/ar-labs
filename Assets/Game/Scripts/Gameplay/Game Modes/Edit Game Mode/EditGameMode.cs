using UnityEngine;

namespace Game.Gameplay
{
    public class EditGameMode : GameMode
    {
        public override void Enable()
        {
            Debug.Log("EditGameMode: enabled");
        }

        public override void Disable()
        {
            Debug.Log("EditGameMode: disabled");
        }
    }
}
