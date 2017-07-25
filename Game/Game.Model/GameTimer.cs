using System;
using System.Threading;
using System.Threading.Tasks;

namespace Game.Model
{
    public class GameTimer : ITimer
    {
        public event EventHandler<TimerUpdateEventArgs> TimerUpdateEvent;
        
        private double timeSpeedFactor;
        public DateTime StartTime { get; }
        public int UpdateFrequencyHz { get; }
        public DateTime CurrentGameTime{ get; private set; }
        public GameTimer(int updateTimeFrequencyHz = 1, double speedFactor = 1)
        {
            StartTime = DateTime.Now;
            CurrentGameTime = StartTime;
            timeSpeedFactor = speedFactor;
            UpdateFrequencyHz = updateTimeFrequencyHz;
        }
        public void RunTimerSynchronously()
        {
            long waitTimeInMilliseconds = 1000*UpdateFrequencyHz;
            
            for (;;)
            {
                TimerUpdateEvent?.Invoke(this, new TimerUpdateEventArgs(CurrentGameTime));

                Thread.Sleep((int)waitTimeInMilliseconds);
               
                CurrentGameTime = CurrentGameTime.AddMilliseconds(timeSpeedFactor*waitTimeInMilliseconds);
            }
        }
        public Task RunTimerAsync()
        {
            return Task.Factory.StartNew( () =>  RunTimerSynchronously() );
        }
        public void SetTimeSpeedFactor(double factor)
        {
            Interlocked.Exchange(ref timeSpeedFactor, factor);
        }
        public DateTime GetCurrentTime()
        {
            return CurrentGameTime;
        }
    }
}
