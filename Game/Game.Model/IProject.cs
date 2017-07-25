using System;

namespace Game.Model
{
    public interface  IProject
    {
        DateTime ExpiryTime { get; }
        long WorkAmountAssigned{ get; }
        bool IsWorkCompleted { get; }
        long WorkAmountRemaining { get; }
        double WorkCompletionPercentage { get; }
        string Title { get; }
        long  Reward { get; }
        DateTime StartTime { get; }
        bool TrySetStartTime(DateTime time);
        bool IsExpired(DateTime time);
        double GetPercentageTimePassed(DateTime currentTime);
        long DoWorkOnProject(long work);
    }
}
