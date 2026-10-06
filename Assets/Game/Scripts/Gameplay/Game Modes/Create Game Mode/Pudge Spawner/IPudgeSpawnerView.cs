namespace Game.Gameplay
{
    public interface IPudgeSpawnerView
    {
        float InitialScale { get; }

        IPudgeView CreatePudgeObject(Pudge.State pudgeState);
    }
}
