using Game.Core;
using Game.Gameplay;
using NSubstitute;
using R3;

namespace Game.Tests.EditMode
{
    public static class Setup
    {
        public static Pudge Pudge(
            float linearMovementSpeed = 1f,
            float linearRotationSpeed = 1f,
            float linearScaleSpeed = 1f)
        {
            return Pudge(out _, out _, linearMovementSpeed, linearRotationSpeed, linearScaleSpeed);
        }

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

        public static Timer Timer(out Subject<Tick> onTick)
        {
            onTick = new Subject<Tick>();

            var tickService = Substitute.For<ITickService>();
            tickService.OnTick.Returns(onTick);

            Timer timer = new(tickService);
            return timer;
        }

        public static CrossDetector CrossDetector(
            float maxDiagonalDeltaAngle = 30f,
            float maxDeltaTimeBetweenTwoSwipes = 1f)
        {
            return CrossDetector(out _, maxDiagonalDeltaAngle, maxDeltaTimeBetweenTwoSwipes);
        }

        public static CrossDetector CrossDetector(
            out ReactiveProperty<bool> timerElapsed,
            float maxDiagonalDeltaAngle = 30f,
            float maxDeltaTimeBetweenTwoSwipes = 1f)
        {
            timerElapsed = new ReactiveProperty<bool>(false);

            var crossDetectorView = Substitute.For<ICrossDetectorView>();
            crossDetectorView.MaxDiagonalDeltaAngle.Returns(maxDiagonalDeltaAngle);
            crossDetectorView.MaxDeltaTimeBwetweenTwoSwipes.Returns(maxDeltaTimeBetweenTwoSwipes);

            var timer = Substitute.For<ITimer>();
            timer.Elapsed.Returns(timerElapsed);

            var timerService = Substitute.For<ITimerService>();
            timerService.CreateTimer().Returns(timer);

            CrossDetector crossDetector = new(crossDetectorView, timerService);
            return crossDetector;
        }
    }
}