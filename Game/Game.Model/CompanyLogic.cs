using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Game.Model
{
    using DevContainer = EntityRepository<IDeveloper>;
    using ProjContainer = EntityRepository<IProject>;

    public class CompanyLogic : ICompanyLogic
    {
        public void PerformOneWorkDayOnProjects(DevContainer developers, ProjContainer projects)
        {
            // projects with the the lowest workAmountRest are processed first
            var sortedProjects = projects.Values.ToList();
            sortedProjects.Sort((p1, p2) => p1.WorkAmountRemaining.CompareTo(p2.WorkAmountRemaining));
            
            int totalProductivityPerDay = developers.Sum(x => x.CodeLinesPerDay);
            long unusedWork = 0;

            for (int i = 0; i < projects.Count; ++i)
                unusedWork = sortedProjects[i].DoWorkOnProject((totalProductivityPerDay / projects.Count) + unusedWork);
        }
        public void PaySalariesAndRemoveUnpaidDevs(DevContainer developers, ref long currentBudget)
        {
            var unpaidDevs = new List<IDeveloper>();

            foreach (var d in developers)
            {
                if (d.MonthlySalary > currentBudget)
                    unpaidDevs.Add(d);
                else
                    Interlocked.Add(ref currentBudget, -d.MonthlySalary);
            }

            unpaidDevs.ForEach(c => developers.TryRemove(c));
        }
        public void RemoveExpiredProjectsAndUpdateTimeCompletionStatus(ProjContainer projects, DateTime time)
        {
            var expiredProjects = new List<IProject>();

            foreach (var p in projects)
            {
                if (DateTime.Compare(time, p.ExpiryTime) > 0)
                    expiredProjects.Add(p);
            }

            expiredProjects.ForEach(c => projects.TryRemove(c));
        }
        public void RemoveResignedDevelopers(DevContainer developers, DateTime time)
        {
            var resignedDevs = new List<IDeveloper>();

            foreach (var d in developers)
            {
                var fireDate = d.FireDate;

                if (fireDate.HasValue && DateTime.Compare(fireDate.Value.Date, time.Date) <= 0)
                    resignedDevs.Add(d);
            }

            foreach (var d in resignedDevs)
            {
                developers.TryRemove(d);
            }
        }
        public void RemoveFinishedProjectAndGetTheRestReward(ProjContainer projects, DateTime currentTime, ref long currentBudget)
        {
            var finishedProjects = new List<IProject>();

            foreach (var p in projects)
            {
                if (p.IsWorkCompleted)
                    finishedProjects.Add(p);
            }

            foreach (var p in finishedProjects)
            {
                bool result = projects.TryRemove(p);
                
                if (result)
                {
                    long amountAlreadyPaid = GetRewardReceivedFromProjectAtTime(p, currentTime);
                    Interlocked.Add(ref currentBudget, p.Reward - amountAlreadyPaid);
                }
            }
        }
        public void GetRevenueFromOneWorkDay(ProjContainer projects, ref long currentBudget)
        {
            foreach (var p in projects)
            {
                long projectPerDayRevenue = p.Reward / (p.ExpiryTime.Date.Subtract(p.StartTime.Date).Days);
                currentBudget += projectPerDayRevenue;
            }
        }
        public void PunishBudgetForExpiredProject(ProjContainer projects, DateTime lastBookedTime, ref long budget)
        {
            foreach (var p in projects)
            {
                if (p.IsExpired(lastBookedTime))
                    Interlocked.Add(ref budget, -p.Reward);
            }
        }

        public long GetRewardReceivedFromProjectAtTime(IProject p, DateTime currentTime)
        {
            return  p.Reward * (currentTime.Subtract(p.StartTime.Date).Days) / (p.ExpiryTime.Subtract(p.StartTime.Date).Days);

        }
    }
}
