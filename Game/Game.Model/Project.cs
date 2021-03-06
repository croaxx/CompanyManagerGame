﻿using System;

namespace Game.Model
{
    public class ProjectEventArgs : EventArgs
    {
        public IProject project { get; }
        public ProjectEventArgs(IProject args)
        {
            project = args;
        }
    }

    public class Project : IProject
    {
        private bool isStartTimeSet = false;

        private Project(DateTime expiry)
        {
            ExpiryTime = expiry;
        }
        private Project(string title, DateTime expiry) : this(expiry)
        {
            Title = title;
        }
        public Project(string title, DateTime expiry, double reward, long workAmountAssigned) : this(title, expiry)
        {
            Reward = reward;
            WorkAmountAssigned = workAmountAssigned;
            WorkAmountRemaining = WorkAmountAssigned;
            WorkCompletionPercentage = 0.0;
            IsWorkCompleted = WorkAmountRemaining == 0 ? true : false;
        }

        public DateTime ExpiryTime { get; }
        public long WorkAmountAssigned { get; }
        public bool IsWorkCompleted { get; private set; }
        public long WorkAmountRemaining { get; private set; }
        public double WorkCompletionPercentage { get; private set; }
        public bool IsOngoing { get; private set; } = true;
        public void SetOnGoingStatusToFalse()
        {
            IsOngoing = false;
        }
        public string Title { get; }
        public double  Reward { get; }
        public DateTime StartTime { get; private set; }
        public bool TrySetStartTime(DateTime time)
        {
            if (!isStartTimeSet)
            {
                if (DateTime.Compare(time, ExpiryTime) >= 0)
                    return false;

                StartTime = time;
                isStartTimeSet = true;
                return true;
            }

            return false;
        }
        public bool IsExpired(DateTime current)
        {
            return DateTime.Compare(ExpiryTime, current) < 0 ? true : false;
        }
        public double GetPercentageTimePassed(DateTime currentTime)
        {
            long elapsedTicks = currentTime.Ticks - StartTime.Ticks;
            long totalProjectDuration = ExpiryTime.Ticks - StartTime.Ticks;

            return 100.0 * elapsedTicks / totalProjectDuration;
        }
        public long DoWorkOnProject(long work)
        {
            long unusedWork = work - WorkAmountRemaining;

            if (unusedWork >= 0)
            {
                WorkAmountRemaining = 0;
                IsWorkCompleted = true;
            }
            else
            {
                unusedWork = 0;
                WorkAmountRemaining -= work;
            }

            WorkCompletionPercentage = 100.0*(WorkAmountAssigned - WorkAmountRemaining)/WorkAmountAssigned;
            
            return unusedWork;
        }
        public override bool Equals(object obj)
        {
            Project d = obj as Project;

            if (d == null) return false;

            return d.Title == Title;
        }
        public override int GetHashCode()
        {
            return Title.GetHashCode();
        }
    }
}
