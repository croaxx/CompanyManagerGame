using System;
using System.Threading;
using System.Threading.Tasks;

namespace Game.Model
{
    public class GameTimer : ITimer
    {
        public event EventHandler<TimerUpdateEventArgs> TimerUpdateEvent;
        
        public DateTime StartTime { get; private set; }

        public int UpdateTimeFrequencyHz { get; private set; }
        
        public DateTime CurrentGameTime{ get; private set; }

        private double timeSpeedFactor;

        public double TimeSpeedFactor { get { return timeSpeedFactor;} }

        public GameTimer(int updateTimeFrequencyHz = 1, double speedFactor = 1)
        {
            this.StartTime = DateTime.Now;
            this.CurrentGameTime = this.StartTime;
            this.timeSpeedFactor = speedFactor;
            this.UpdateTimeFrequencyHz = updateTimeFrequencyHz;
        }

        public void RunTimerSynchronously()
        {
            int waitTimeInMilliseconds = 1000*UpdateTimeFrequencyHz;
            
            for (;;)
            {
                TimerUpdateEvent?.Invoke(this, new TimerUpdateEventArgs(CurrentGameTime));

                Thread.Sleep(waitTimeInMilliseconds);

                CurrentGameTime = CurrentGameTime.AddMilliseconds(TimeSpeedFactor*waitTimeInMilliseconds);
            }
        }

        public Task LaunchAsync()
        {
            return Task.Factory.StartNew( () =>  RunTimerSynchronously() );
        }

        public void SetTimeSpeedFactor(double factor)
        {
            this.timeSpeedFactor = factor;
        }

        public DateTime GetCurrentTime()
        {
            return CurrentGameTime;
        }
    }
}
