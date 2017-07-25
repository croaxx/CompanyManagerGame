using System;
using System.Collections.Concurrent;

namespace Game.Model
{
    using DevContainer = EntityRepository<IDeveloper>;
    using ProjContainer = EntityRepository<IProject>;

    public interface ICompanyLogic
    {
        void RemoveResignedDevelopers(DevContainer developers, DateTime time);
        void PaySalariesAndRemoveUnpaidDevs(DevContainer developers, ref long currentBudget);
        void PerformOneWorkDayOnProjects(DevContainer developers, ProjContainer projects);
        void RemoveExpiredProjectsAndUpdateTimeCompletionStatus(ProjContainer projects, DateTime time);
        void RemoveFinishedProjectAndGetTheRestReward(ProjContainer projects, DateTime currentTime, ref long currentBudget);
        void GetRevenueFromOneWorkDay(ProjContainer projects, ref long currentBudget);
        void PunishBudgetForExpiredProject(ProjContainer projects, DateTime lastBookedTime, ref long budget);
        long GetRewardReceivedFromProjectAtTime(IProject proj, DateTime currentTime);
    }
}
