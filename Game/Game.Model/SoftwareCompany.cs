using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Game.Model
{
    public class NextSalaryPaymentEventArgs : EventArgs
    {
        public DateTime DateTimeArgs { get; }
        public NextSalaryPaymentEventArgs(DateTime args)
        {
            DateTimeArgs = args;
        }
    }

    public class SoftwareCompany : ICompany
    {
        private ConcurrentDictionary<string, Project> projects;
        private ConcurrentDictionary<Developer, Developer> developers;

        public event EventHandler<EventArgs> ProjectsCollectionChange;
        public event EventHandler<EventArgs> DevelopersCollectionChange;
        public event EventHandler<EventArgs> BudgetChange;

        public string Name { get; private set; }
        public DateTime FoundationDate { get; private set; }
        public DateTime NextSalaryPaymentDate { get; private set; }

        private long currentBudget;
        public DateTime LastBookedTime { get; private set; }
        private void SetNextSalaryPaymentDate()
        {
            this.NextSalaryPaymentDate = this.NextSalaryPaymentDate.IncrementMonths(1);
        }

        public SoftwareCompany()
        {
            this.projects = new ConcurrentDictionary<string, Project>();
            this.developers = new ConcurrentDictionary<Developer, Developer>();
            this.currentBudget = 100000;
        }

        private void SetFirstSalaryPaymentDateTo(int day)
        {
            var date = FoundationDate.IncrementMonths(1);

            this.NextSalaryPaymentDate = date.AddDays(day - date.Day);
        }

        public SoftwareCompany(DateTime time, int salaryDay = 25) : this()
        {
            this.FoundationDate = time;
            this.LastBookedTime = this.FoundationDate;
            SetFirstSalaryPaymentDateTo(salaryDay);
        }

        private void OnSalaryPayment(object sender, EventArgs e)
        {
            SetNextSalaryPaymentDate();
        }

        public bool TryAcceptNewProject(Project proj, DateTime startTime)
        {
            bool result = proj.TrySetStartTime(startTime);

            if (result)
            {
                bool status = projects.TryAdd(proj.Title, proj);

                if (status)
                    ProjectsCollectionChange?.Invoke(this, new EventArgs());

                return status;
            }
            else
                return false;
        }

        public void FireDeveloperByFullNameAndBirthdate(string fullname)
        {
            //developers.Where( x => x.FullName == fullname ).
        }

        public string GetCompanyName()
        {
            return Name;
        }

        public long GetCompanyBudget()
        {
            return currentBudget;
        }

        public int GetNumberOfProjects()
        {
            return projects.Count;
        }

        public void QuitProject(string title)
        {
            projects.TryRemove(title, out Project value);
            ProjectsCollectionChange?.Invoke(this, new EventArgs());
        }

        public IList<Project> GetProjects()
        {
            var p = new List<Project>();

            foreach (var item in projects)
            {
                p.Add(item.Value);
            }

            return p;
        }

        public IList<Developer> GetDevelopers()
        {
            var d = new List<Developer>();

            foreach (var item in developers)
            {
                d.Add(item.Key);
            }

            return d;
        }

        public void UpdateCompanyStatus(DateTime currentTime, ICompanyLogic logic)
        {
            while (LastBookedTime.Date != currentTime.Date)
            {
                if (LastBookedTime.Date == NextSalaryPaymentDate.Date)
                {
                    logic.PaySalariesAndRemoveUnpaidDevs(developers, ref currentBudget);
                    SetNextSalaryPaymentDate();
                }
                
                logic.RemoveExpiredProjectsAndUpdateTimeStatus(projects, LastBookedTime);
                logic.RemoveResignedDevelopers(developers, LastBookedTime);
                logic.PerformOneWorkDayOnProjects(developers, projects);

                LastBookedTime = LastBookedTime.AddHours(24);
            }

            ProjectsCollectionChange?.Invoke(this, new EventArgs());
            DevelopersCollectionChange?.Invoke(this, new EventArgs());
            BudgetChange?.Invoke(this, new EventArgs());
        }

        public bool TryHireDeveloper(Developer d)
        {
            bool status = developers.TryAdd(d, d);

            if (status)
                DevelopersCollectionChange?.Invoke(this, new EventArgs());
            
            return status;
        }

        public int GetNumberOfDevelopers()
        {
            return this.developers.Count;
        }

        public void ResignDeveloperAfterMonths(Developer d, DateTime currenttime, int monthsspan)
        {
            d.Resign(currenttime.AddMonths(monthsspan));
            //developers.TryRemove(d, out object value);
            developers.AddOrUpdate(d, d, (key, oldValue) => d);
            DevelopersCollectionChange?.Invoke(this, new EventArgs());
        }

        public DateTime GetNextSalaryPaymentDate()
        {
            return this.NextSalaryPaymentDate;
        }
    }
}
