using System;
using System.Collections.Generic;

namespace Game.Model
{
    using DevContainer = EntityCollection<IDeveloper>;
    using ProjContainer = EntityCollection<IProject>;

    public interface ICompanyLogic
    {
        IEnumerable<IDeveloper> RemoveResignedDevelopers(DevContainer developers, DateTime time);
        IEnumerable<IDeveloper> PaySalariesAndRemoveUnpaidDevs(DevContainer developers, ref double currentBudget);
        void PerformOneWorkDayOnProjects(DevContainer developers, ProjContainer projects);
        IEnumerable<IProject> RemoveExpiredProjects(ProjContainer projects, DateTime time);
        IEnumerable<IProject> RemoveFinishedProjectAndGetTheRestReward(ProjContainer projects, DateTime currentTime, ref double currentBudget);
        IEnumerable<IProject> RemoveStoppedProjectsAndPunishBudget(ProjContainer projects, DateTime currentTime, ref double currentBudget);
        void GetRevenueFromOneWorkDay(ProjContainer projects, ref double currentBudget);
        void PunishBudgetForExpiredProject(ProjContainer projects, DateTime lastBookedTime, ref double budget);
        double GetRewardReceivedFromProjectAtDate(IProject proj, DateTime currentTime);
        
    }
}
