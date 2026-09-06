using Game.Core;
using UnityEngine;

namespace Game.Lab1
{
    public class Pudge : MonoBehaviour
    {
        private readonly PudgeView _pudgeView;

        public Pudge(PudgeView pudgeView)
        {
            _pudgeView = pudgeView;
        }

        public void PlayRandomAnimation()
        {
            _pudgeView.PlayRandomAnimation();
        }

        public void Despawn()
        {
            _pudgeView.DestroyObject();
        }
    }
}
