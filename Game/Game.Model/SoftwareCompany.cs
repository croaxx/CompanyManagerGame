using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Game.Model
{
    public class EntityCollection<T> : IEnumerable<T>
    {
        private ConcurrentDictionary<T, T> entity;
        public EntityCollection()
        {
            entity = new ConcurrentDictionary<T, T>();
        }
        public ICollection<T> Values { get { return entity.Values;} }
        public int Count { get { return entity.Count;} }
        public IEnumerator<T> GetEnumerator()
        {
            return entity.Values.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public bool TryRemove(T c)
        {
            return entity.TryRemove(c, out T value);
        }
        public bool TryAdd(T proj)
        {
            return entity.TryAdd(proj, proj);
        }
    }

    public class SoftwareCompany : ICompany
    {
        public event EventHandler<ProjectEventArgs> ProjectAdded;
        public event EventHandler<ProjectEventArgs> ProjectRemoved;

        public event EventHandler<DeveloperEventArgs> DeveloperAdded;
        public event EventHandler<DeveloperEventArgs> DeveloperRemoved;

        public event EventHandler<EventArgs> BudgetChanged;

        private EntityCollection<IProject> projects;
        private EntityCollection<IDeveloper> developers;
        private double currentBudget;

        public string Name { get; }
        public DateTime FoundationDate { get; }
        public DateTime NextSalaryPaymentDate { get; private set; }
        public DateTime LastBookedTime { get; private set; }
        public SoftwareCompany(int budget = 200000)
        {
            projects = new EntityCollection<IProject>();
            developers = new EntityCollection<IDeveloper>();;
            currentBudget = budget;
        }
        private void SetFirstSalaryPaymentDateTo(int day)
        {
            var date = FoundationDate.TryIncrementMonths(1).Value;
            NextSalaryPaymentDate = date.AddDays(day - date.Day);
        }
        public SoftwareCompany(DateTime time) : this()
        {
            int salaryDay = 25;
            FoundationDate = time;
            LastBookedTime = FoundationDate;
            SetFirstSalaryPaymentDateTo(salaryDay);
        }
        public bool TryAcceptNewProject(IProject proj, DateTime startTime)
        {
            bool result = proj.TrySetStartTime(startTime);

            if (result)
            {
                bool status = projects.TryAdd(proj);

                if (status)
                    ProjectAdded?.Invoke(this, new ProjectEventArgs(proj));

                return status;
            }
            else
                return false;
        }
        public string GetCompanyName()
        {
            return Name;
        }
        public double GetCompanyBudget()
        {
            return currentBudget;
        }
        public int GetNumberOfProjects()
        {
            return projects.Count;
        }
        public void SetProjectOngoingStatusToFalse(IProject proj)
        {
            proj.SetOnGoingStatusToFalse();
        }
        public IEnumerable<IProject> GetProjects()
        {
            return projects.Values;
        }
        public IEnumerable<IDeveloper> GetDevelopers()
        {
            return developers.Values;
        }
        public void UpdateCompanyStatus(DateTime currentTime, ICompanyLogic logic)
        {
            while (LastBookedTime.Date < currentTime.Date)
            {
                if (LastBookedTime.Date == NextSalaryPaymentDate.Date)
                {
                    var unpaidDevs = logic.PaySalariesAndRemoveUnpaidDevs(developers, ref currentBudget);
                    
                    foreach (var d in unpaidDevs)
                        DeveloperRemoved?.Invoke(this, new DeveloperEventArgs(d));
                    
                    NextSalaryPaymentDate = NextSalaryPaymentDate.TryIncrementMonths(1).Value;
                }

                logic.GetRevenueFromOneWorkDay(projects, ref currentBudget);

                var stoppedProjects = logic.RemoveStoppedProjectsAndPunishBudget(projects, LastBookedTime, ref currentBudget);
                
                foreach (var p in stoppedProjects)
                    ProjectRemoved?.Invoke(this, new ProjectEventArgs(p));

                logic.PunishBudgetForExpiredProject(projects, LastBookedTime, ref currentBudget);
                
                var expiredProjects = logic.RemoveExpiredProjects(projects, LastBookedTime);

                foreach (var p in expiredProjects)
                    ProjectRemoved?.Invoke(this, new ProjectEventArgs(p));
                
                var resignedDevs = logic.RemoveResignedDevelopers(developers, LastBookedTime);
                
                foreach (var d in resignedDevs)
                    DeveloperRemoved?.Invoke(this, new DeveloperEventArgs(d));

                logic.PerformOneWorkDayOnProjects(developers, projects);
                
                var finishedProjects = logic.RemoveFinishedProjectAndGetTheRestReward(projects, LastBookedTime, ref currentBudget);
                
                foreach (var p in finishedProjects)
                    ProjectRemoved?.Invoke(this, new ProjectEventArgs(p));

                LastBookedTime = LastBookedTime.AddDays(1);
            }

            BudgetChanged?.Invoke(this, new EventArgs());
        }
        public bool TryHireDeveloper(IDeveloper d)
        {
            bool status = developers.TryAdd(d);

            if (status)
                DeveloperAdded?.Invoke(this, new DeveloperEventArgs(d));
            
            return status;
        }
        public int GetNumberOfDevelopers()
        {
            return developers.Count;
        }
        public void SetDeveloperResignationTime(IDeveloper d, DateTime time)
        {
            d.Resign(time);
        }
        public DateTime GetNextSalaryPaymentDate()
        {
            return NextSalaryPaymentDate;
        }
    }
}
