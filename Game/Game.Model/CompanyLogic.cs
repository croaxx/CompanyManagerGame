using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Game.Model
{
    using DevContainer = EntityCollection<IDeveloper>;
    using ProjContainer = EntityCollection<IProject>;

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
        public IEnumerable<IDeveloper> PaySalariesAndRemoveUnpaidDevs(DevContainer developers, ref double currentBudget)
        {
            var unpaidDevs = new List<IDeveloper>();

            foreach (var d in developers)
            {
                if (d.MonthlySalary > currentBudget)
                    unpaidDevs.Add(d);
                else
                    Interlocked.Exchange(ref currentBudget, currentBudget - d.MonthlySalary);
            }

            unpaidDevs.ForEach(c => developers.TryRemove(c));
            
            return unpaidDevs;
        }
        public IEnumerable<IProject> RemoveExpiredProjects(ProjContainer projects, DateTime time)
        {
            var expiredProjects = new List<IProject>();

            foreach (var p in projects)
            {
                if (DateTime.Compare(time.Date, p.ExpiryTime.Date) > 0)
                    expiredProjects.Add(p);
            }

            expiredProjects.ForEach(c => projects.TryRemove(c));

            return expiredProjects;
        }
        public IEnumerable<IDeveloper> RemoveResignedDevelopers(DevContainer developers, DateTime time)
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

            return resignedDevs;
        }
        public IEnumerable<IProject> RemoveFinishedProjectAndGetTheRestReward(ProjContainer projects, DateTime currentTime, ref double currentBudget)
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
                    double amountAlreadyPaid = GetRewardReceivedFromProjectAtDate(p, currentTime.Date);
                    Interlocked.Exchange(ref currentBudget, currentBudget + p.Reward - amountAlreadyPaid);
                }
            }

            return finishedProjects;
        }
        public void GetRevenueFromOneWorkDay(ProjContainer projects, ref double currentBudget)
        {
            foreach (var p in projects)
            {
                double projectPerDayRevenue = p.Reward / (p.ExpiryTime.Date.Subtract(p.StartTime.Date).Days);
                currentBudget += projectPerDayRevenue;
            }
        }
        public void PunishBudgetForExpiredProject(ProjContainer projects, DateTime lastBookedTime, ref double budget)
        {
            foreach (var p in projects)
            {
                if (p.IsExpired(lastBookedTime.Date))
                    Interlocked.Exchange(ref budget, budget - p.Reward);
            }
        }
        public double GetRewardReceivedFromProjectAtDate(IProject p, DateTime currentDate)
        {
            var startDate = p.StartTime.Date;
            var expiryDate = p.ExpiryTime.Date;

            return  p.Reward * (currentDate.Subtract(startDate).Days) / (expiryDate.Subtract(startDate).Days);
        }
        public IEnumerable<IProject> RemoveStoppedProjectsAndPunishBudget(ProjContainer projects, DateTime currentTime, ref double currentBudget)
        {
            var stoppedProjects = new List<IProject>();

            foreach (var p in projects)
            {
                if (!p.IsOngoing)
                    stoppedProjects.Add(p);
            }

            foreach (var p in stoppedProjects)
            {
                bool result = projects.TryRemove(p);
                
                if (result)
                {
                    double amountAlreadyPaid = GetRewardReceivedFromProjectAtDate(p, currentTime.Date);
                    Interlocked.Exchange(ref currentBudget, currentBudget - amountAlreadyPaid);
                }
            }

            return stoppedProjects;
        }
    }
}
