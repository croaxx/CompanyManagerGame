using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Game.Model
{
    public class CompanyLogic : ICompanyLogic
    {
        public void PerformOneWorkDayOnProjects(ConcurrentDictionary<Developer, Developer> developers, ConcurrentDictionary<string, Project> projects)
        {
            int totalRemainingProductivityPerDay = developers.Sum(x => x.Value.CodeLinesPerDay);
            int projectsCount = projects.Count;

            int unusedWork = 0;

            var p = projects.GetEnumerator();
            p.MoveNext();
            for (int i = 0; i < projects.Count; ++i)
            {
                unusedWork = p.Current.Value.DoWorkOnProject((totalRemainingProductivityPerDay / projectsCount) + unusedWork);
                p.MoveNext();
            }
        }

        public void PaySalariesAndRemoveUnpaidDevs(ConcurrentDictionary<Developer, Developer> developers, ref long currentBudget)
        {
            var unpaidDevs = new List<Developer>();

            foreach (var d in developers)
            {
                if (d.Value.MonthlySalary > currentBudget)
                    unpaidDevs.Add(d.Value);
                else
                    currentBudget -= d.Value.MonthlySalary;
            }

            foreach (var d in unpaidDevs)
            {
                developers.TryRemove(d, out Developer value);
            }
        }

        public void RemoveExpiredProjectsAndUpdateTimeStatus(ConcurrentDictionary<string, Project> projects, DateTime time)
        {
            var expiredProjects = new List<string>();

            foreach (var p in projects)
            {
                if (DateTime.Compare(time, p.Value.ExpiryTime) > 0)
                    expiredProjects.Add(p.Key);

                p.Value.UpdatePercentTimePassed(time);
            }

            foreach (var p in expiredProjects)
            {
                projects.TryRemove(p, out Project value);
            }
        }

        public void RemoveResignedDevelopers(ConcurrentDictionary<Developer, Developer> developers, DateTime time)
        {
            var resignedDevs = new List<Developer>();

            foreach (var d in developers)
            {
                DateTime? fireDate = d.Key.FireDate;
                if (fireDate != null && DateTime.Compare(fireDate.Value, time) <= 0)
                {
                    resignedDevs.Add(d.Value);
                }
            }

            foreach (var d in resignedDevs)
            {
                developers.TryRemove(d, out Developer dev);
            }

        }
    }
}
