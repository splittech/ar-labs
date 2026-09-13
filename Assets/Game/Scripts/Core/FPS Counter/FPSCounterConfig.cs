using UnityEngine;

namespace Game.Core
{
    [CreateAssetMenu(fileName = "FPS Counter Config", menuName = "Game/Configs/FPS Counter Config", order = 0)]
    public class FPSCounterConfig : ScriptableObject
    {
        public float TimeBetweenFPSTextUpdate = 1f;
    }
}