using System;
using System.Collections.Concurrent;

namespace Game.Model
{
    public interface ICompanyLogic
    {
        void RemoveResignedDevelopers(ConcurrentDictionary<Developer, Developer> developers, DateTime time);
        void PaySalariesAndRemoveUnpaidDevs(ConcurrentDictionary<Developer, Developer> developers, ref long currentBudget);
        void PerformOneWorkDayOnProjects(ConcurrentDictionary<Developer, Developer> developers, ConcurrentDictionary<string, Project> projects);
        void RemoveExpiredProjectsAndUpdateTimeStatus(ConcurrentDictionary<string, Project> projects, DateTime time);
    }
}
