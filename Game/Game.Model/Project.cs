﻿using System;

namespace Game.Model
{
    public class Project
    {
        public DateTime ExpiryTime { get; private set; }
        public long WorkAmountAssigned { get; private set; }
        public long WorkAmountDone { get; private set; }
        public string Title { get; private set; }
        public long  Reward { get; private set; }

        private bool isStartTimeSet = false;
        public DateTime StartTime { get; private set; }
        
        public double PercentTimePassed { get; private set; } 
        
        
        public Project() {}
        public Project(string title, DateTime expiry)
        {
            this.Title = title;
            this.ExpiryTime = expiry;
        }
        public Project(DateTime expiry)
        {
            this.ExpiryTime = expiry;
        }
        public Project(string title, DateTime expiry,
                       long reward, long workAmountAssigned)
        {
            this.Title = title;
            this.ExpiryTime = expiry;
            this.Reward = reward;
            this.WorkAmountAssigned = workAmountAssigned;
        }
        
        
        public bool TrySetStartTime(DateTime time)
        {
            if (!isStartTimeSet)
            {
                if ( DateTime.Compare(time, ExpiryTime ) > 0)
                    return false;

                StartTime = time;
                isStartTimeSet = true;
                return true;
            }

            return false;
        }
        public bool IsExpired(DateTime current)
        {
            return DateTime.Compare(this.ExpiryTime, current) < 0 ? true : false;
        }

        public void SetPercentTimePassed(DateTime currentTime)
        {
            long elapsedTicks = currentTime.Ticks - StartTime.Ticks;
            long totalProjectDuration = ExpiryTime.Ticks - StartTime.Ticks;

            this.PercentTimePassed = 100.0*(double)elapsedTicks / (double)totalProjectDuration;
        }
    }
}
