using FakeItEasy;
using FluentAssertions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Game.Model.xUnitTesting
{
    public class GameTimerxUnitTesting
    {
        private ITimer timer;

        [Fact]
        public void ITimerFiresAnEvent_WithSpecificDateTimeStoredInEventArgs()
        {
            timer = A.Fake<ITimer>();
            var expectedDate = new DateTime(2017, 5, 20);
            DateTime date = DateTime.MinValue;
            timer.TimerUpdateEvent += (o, e) => { date = e.TimerArgs; };

            timer.TimerUpdateEvent += Raise.With(new TimerUpdateEventArgs(new DateTime(2017, 5, 20)));

            date.Should().Be(expectedDate);
        }

        [Fact]
        public void ITimerLaunchAsync_ReturnsTask()
        {
            timer = new GameTimer();
            Func<Task> func = () => { return timer.RunTimerAsync(); };

            var tsk = func();

            tsk.Should().NotBeNull();
        }

        [Fact]
        public void LaunchAsync_FiresAnEventAfterUpdateTime()
        {
            int updateFrequencyHz = 10;
            int updateTimeInMilliseconds = 1000/updateFrequencyHz;
            int eventFireCounter = 0;
            timer = new GameTimer(updateFrequencyHz);
            timer.TimerUpdateEvent += ( o, e ) => { eventFireCounter++; };

            var task = timer.RunTimerAsync();

            Thread.Sleep(updateTimeInMilliseconds);
            eventFireCounter.Should().NotBe(0);
        }
    }
}
