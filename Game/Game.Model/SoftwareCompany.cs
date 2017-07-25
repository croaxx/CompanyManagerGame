using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Game.Model
{
    public class EntityRepository<T> : IEnumerable<T>
    {
        private ConcurrentDictionary<T, T> entity;
        public EntityRepository()
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
        public event EventHandler<EventArgs> ProjectsCollectionChange;
        public event EventHandler<EventArgs> DevelopersCollectionChange;
        public event EventHandler<EventArgs> BudgetChange;

        private EntityRepository<IProject> projects;
        private EntityRepository<IDeveloper> developers;
        private long currentBudget;

        public string Name { get; }
        public DateTime FoundationDate { get; }
        public DateTime NextSalaryPaymentDate { get; private set; }
        public DateTime LastBookedTime { get; private set; }
        public SoftwareCompany(int budget = 200000)
        {
            projects = new EntityRepository<IProject>();
            developers = new EntityRepository<IDeveloper>();;
            currentBudget = budget;
        }
        private void SetFirstSalaryPaymentDateTo(int day)
        {
            var date = FoundationDate.TryIncrementMonths(1).Value;
            NextSalaryPaymentDate = date.AddDays(day - date.Day);
        }
        public SoftwareCompany(DateTime time, int salaryDay = 25) : this()
        {
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
                    ProjectsCollectionChange?.Invoke(this, new EventArgs());

                return status;
            }
            else
                return false;
        }
        public string GetCompanyName()
        {
            return Name;
        }
        public long GetCompanyBudget()
        {
            return Interlocked.Read(ref currentBudget);
        }
        public int GetNumberOfProjects()
        {
            return projects.Count;
        }
        public bool TryQuitProjectAndPunishBudget(IProject proj)
        {
            bool isRemoved = projects.TryRemove(proj);
            
            if (isRemoved)
            {
                Interlocked.Add(ref currentBudget, -proj.Reward);
                BudgetChange?.Invoke(this, new EventArgs());
                return true;
            }

            return false;
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
                    logic.PaySalariesAndRemoveUnpaidDevs(developers, ref currentBudget);
                    NextSalaryPaymentDate = NextSalaryPaymentDate.TryIncrementMonths(1).Value;
                }
                
                logic.PunishBudgetForExpiredProject(projects, LastBookedTime, ref currentBudget);
                logic.RemoveExpiredProjectsAndUpdateTimeCompletionStatus(projects, LastBookedTime);
                logic.RemoveResignedDevelopers(developers, LastBookedTime);
                logic.PerformOneWorkDayOnProjects(developers, projects);
                logic.GetRevenueFromOneWorkDay(projects, ref currentBudget);
                logic.RemoveFinishedProjectAndGetTheRestReward(projects, LastBookedTime, ref currentBudget);

                LastBookedTime = LastBookedTime.AddDays(1);
            }

            ProjectsCollectionChange?.Invoke(this, new EventArgs());
            DevelopersCollectionChange?.Invoke(this, new EventArgs());
            BudgetChange?.Invoke(this, new EventArgs());
        }
        public bool TryHireDeveloper(IDeveloper d)
        {
            bool status = developers.TryAdd(d);

            if (status)
                DevelopersCollectionChange?.Invoke(this, new EventArgs());
            
            return status;
        }
        public int GetNumberOfDevelopers()
        {
            return developers.Count;
        }
        public void SetDeveloperResignationDateAfterMonths(IDeveloper d, DateTime currenttime, int monthsspan)
        {
            d.Resign(currenttime.AddMonths(monthsspan));
            DevelopersCollectionChange?.Invoke(this, new EventArgs());
        }
        public DateTime GetNextSalaryPaymentDate()
        {
            return NextSalaryPaymentDate;
        }
    }
}
