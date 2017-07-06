using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Game.Model
{
    public class SoftwareCompany : ICompany
    {
        private ConcurrentDictionary<string, Project> projects;

        public event EventHandler<EventArgs> ProjectsCollectionChange;

        public string Name { get; private set; }

        public long CurrentBudget { get; private set; }

        public SoftwareCompany()
        {
            this.projects = new ConcurrentDictionary<string, Project>();
        }

        public bool TryAcceptNewProject(Project proj, DateTime startTime)
        {
            bool result = proj.TrySetStartTime(startTime);
            
            if (result)
            {
                return projects.TryAdd(proj.Title, proj);;
            }
            else
                return false;
        }

        public void FireDeveloperByFullName(string fullname)
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
    }
}
