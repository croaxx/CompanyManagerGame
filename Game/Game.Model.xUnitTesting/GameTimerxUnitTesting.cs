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
        [Fact]
        public void ITimerFiresAnEventWithSpecificEventArgs()
        {
            var timer = A.Fake<ITimer>();
            var expectedDate = new DateTime(2017, 5, 20);

            DateTime date = DateTime.Now; // set any time

            timer.TimerUpdateEvent += (o, e) => { date = e.TimerArgs; };

            timer.TimerUpdateEvent += Raise.With(new TimerUpdateEventArgs(new DateTime(2017, 5, 20)));

            Assert.Equal(date, expectedDate);
        }

        [Fact]
        public void ITimerLaunchAsyncReturnsTask()
        {
            var timer = A.Fake<ITimer>();

            bool isActionCalled = false;
            Action act = () => { isActionCalled = true; };

            A.CallTo(() => timer.LaunchAsync()).Returns(new Task(act));

            var task = timer.LaunchAsync();

            task.Start();
            task.Wait();

            isActionCalled.Should().Be(true);
        }

        [Fact]
        public void RunTimerSynchronously_FiresEventAfterUpdateTime()
        {
            int updateFrequencyHz = 10;
            int updateTimeInMilliseconds = 1000/updateFrequencyHz;
            int eventFireCounter = 0;
            var timer = new GameTimer(updateFrequencyHz);
            timer.TimerUpdateEvent += ( o, e ) => { eventFireCounter++; };

            var task = Task.Factory.StartNew(() => { timer.RunTimerSynchronously(); });

            Thread.Sleep(updateTimeInMilliseconds);
            eventFireCounter.Should().NotBe(0);
        }

        [Fact]
        public void LaunchAsync_FiresAnEventAfterUpdateTime()
        {
            int updateFrequencyHz = 10;
            int updateTimeInMilliseconds = 1000/updateFrequencyHz;
            int eventFireCounter = 0;
            var timer = new GameTimer(updateFrequencyHz);
            timer.TimerUpdateEvent += ( o, e ) => { eventFireCounter++; };

            var task = timer.LaunchAsync();

            Thread.Sleep(updateTimeInMilliseconds);
            eventFireCounter.Should().NotBe(0);
        }
    }
}
