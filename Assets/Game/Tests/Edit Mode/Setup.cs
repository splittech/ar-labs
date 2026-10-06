using Game.Core;
using Game.Gameplay;
using NSubstitute;
using R3;

namespace Game.Tests.EditMode
{
    public static class Setup
    {
        public static Pudge Pudge(
            out Subject<Tick> onTick,
            float linearMovementSpeed = 1f,
            float linearRotationSpeed = 1f,
            float linearScaleSpeed = 1f)
        {
            return Pudge(out onTick, out _, linearMovementSpeed, linearRotationSpeed, linearScaleSpeed);
        }

        public static Pudge Pudge(
            out Subject<Tick> onTick,
            out IPudgeView pudgeView,
            float linearMovementSpeed = 1f,
            float linearRotationSpeed = 1f,
            float linearScaleSpeed = 1f)
        {
            onTick = new Subject<Tick>();

            pudgeView = Substitute.For<IPudgeView>();
            pudgeView.LinearMovementSpeed.Returns(linearMovementSpeed);
            pudgeView.LinearRotationSpeed.Returns(linearRotationSpeed);
            pudgeView.LinearScaleSpeed.Returns(linearScaleSpeed);

            var tickService = Substitute.For<ITickService>();
            tickService.OnTick.Returns(onTick);

            Pudge pudge = new(pudgeView, tickService);
            return pudge;
        }
    }
}