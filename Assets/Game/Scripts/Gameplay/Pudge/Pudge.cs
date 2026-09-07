namespace Game.Gameplay
{
    public class Pudge
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
