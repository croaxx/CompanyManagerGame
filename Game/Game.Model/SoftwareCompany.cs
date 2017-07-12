using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Game.Model
{
    public class SoftwareCompany : ICompany
    {
        private ConcurrentDictionary<string, Project> projects;
        private ConcurrentDictionary<KeyValuePair<string, DateTime>, Developer> developers;

        private IBookingLogic _bookingLogic;

        public event EventHandler<EventArgs> ProjectsCollectionChange;

        public string Name { get; private set; }

        public long CurrentBudget { get; private set; }

        public DateTime LastBookedTime { get; set; }

        public SoftwareCompany(IBookingLogic bookingLogic)
        {
            this._bookingLogic = bookingLogic;
            this.projects = new ConcurrentDictionary<string, Project>();
            LastBookedTime = DateTime.MinValue;
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
            return CurrentBudget;
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

        public void UpdateProjectsStatus(DateTime currentTime)
        {
            if (this.LastBookedTime == DateTime.MinValue)
            {
                this.LastBookedTime = currentTime;
                return;
            }

            if (
                //same month
                (currentTime.Day > LastBookedTime.Day && 
                currentTime.Month == LastBookedTime.Month &&
                currentTime.Year == LastBookedTime.Year ) ||
                //next month
                (currentTime.Month > LastBookedTime.Month &&
                currentTime.Year == LastBookedTime.Year) ||
                //next year
                currentTime.Year > LastBookedTime.Year)
            {
                this._bookingLogic.BookTime(null, null);
            }
            foreach (var p in projects)
            {
                p.Value.SetPercentTimePassed(currentTime);
            }
        
            ProjectsCollectionChange?.Invoke(this, new EventArgs());
        }

        //public bool TryHireDeveloper(string name, DateTime Birthdate)
        //{

        //}
    }
}
