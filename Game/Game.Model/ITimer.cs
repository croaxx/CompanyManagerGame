using System;
using System.Threading.Tasks;

namespace Game.Model
{
    public class TimerUpdateEventArgs : EventArgs
    {
        public DateTime TimerArgs { get; }
        public TimerUpdateEventArgs(DateTime args)
        {
            TimerArgs = args;
        }
    }

    public interface ITimer
    {
        event EventHandler<TimerUpdateEventArgs> TimerUpdateEvent;
        Task LaunchAsync();
        void SetTimeSpeedFactor(double factor);
        DateTime GetCurrentTime();
    }
}
