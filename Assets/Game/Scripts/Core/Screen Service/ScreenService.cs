using UnityEngine;

namespace Game.Core
{
    public class ScreenService : IScreenService
    {
        public float SceenWidth => Screen.width;
        public float SceenHeight => Screen.height;
    }
}