using Game.Core;
using NUnit.Framework;

namespace Game.Tests.EditMode
{
    public class TimerTests
    {
        public const float TimerTime = 1f;

        [Test]
        public void Reset_TicksExceedTime_TimerIsElapsed()
        {
            // Arrange.
            Timer timer = Setup.Timer(out var onTick);

            // Act.
            timer.Reset(TimerTime);
            onTick.OnNext(new Tick(type: TickType.Update, deltaTime: TimerTime * 2f));

            // Assert.
            Assert.That(timer.Elapsed.CurrentValue, Is.True);
        }

        [Test]
        public void Reset_TicksDoNotExceedTime_TimerIsNotElapsed()
        {
            // Arrange.
            Timer timer = Setup.Timer(out var onTick);

            // Act.
            timer.Reset(TimerTime);
            onTick.OnNext(new Tick(type: TickType.Update, deltaTime: TimerTime / 2f));

            // Assert.
            Assert.That(timer.Elapsed.CurrentValue, Is.False);
        }

        [Test]
        public void Reset_CalledAgainBeforeTimeEnded_TimeIsCountedFromStart()
        {
            // Arrange.
            Timer timer = Setup.Timer(out var onTick);
            timer.Reset(TimerTime);
            onTick.OnNext(new Tick(type: TickType.Update, deltaTime: TimerTime * 0.75f));

            // Act.
            timer.Reset(TimerTime);
            onTick.OnNext(new Tick(type: TickType.Update, deltaTime: TimerTime * 0.75f));

            // Assert.
            Assert.That(timer.Elapsed.CurrentValue, Is.False);
        }

        [Test]
        public void Stop_TimerIsElapsed_TimerIsNotElapsed()
        {
            // Arrange.
            Timer timer = Setup.Timer(out var onTick);
            timer.Reset(TimerTime);
            onTick.OnNext(new Tick(type: TickType.Update, deltaTime: TimerTime * 2f));

            // Act.
            timer.Stop();

            // Assert.
            Assert.That(timer.Elapsed.CurrentValue, Is.False);
        }

        [Test]
        public void Stop_TimeNotEndedAndTicksExceedTime_TimerIsNotElapsed()
        {
            // Arrange.
            Timer timer = Setup.Timer(out var onTick);
            timer.Reset(TimerTime);

            // Act.
            timer.Stop();
            onTick.OnNext(new Tick(type: TickType.Update, deltaTime: TimerTime * 2f));

            // Assert.
            Assert.That(timer.Elapsed.CurrentValue, Is.False);
        }

        [Test]
        public void Reset_FixedUpdateTicksExceedTime_TimerIsNotElapsed()
        {
            // Arrange.
            Timer timer = Setup.Timer(out var onTick);

            // Act.
            timer.Reset(TimerTime);
            onTick.OnNext(new Tick(type: TickType.FixedUpdate, deltaTime: TimerTime * 2f));

            // Assert.
            Assert.That(timer.Elapsed.CurrentValue, Is.False);
        }
    }
}
