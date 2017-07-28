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
        bool IsOngoing { get; }
        void SetOnGoingStatusToFalse();
        string Title { get; }
        double  Reward { get; }
        DateTime StartTime { get; }
        bool TrySetStartTime(DateTime time);
        bool IsExpired(DateTime time);
        double GetPercentageTimePassed(DateTime currentTime);
        long DoWorkOnProject(long work);
    }
}
