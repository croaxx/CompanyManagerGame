using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Game.Model
{
    public class SoftwareCompany : ICompany
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private IList<Project> projects;

        public IList<Project> Projects
        { 
            get
            {
                return projects;
            }
            set
            {
                projects = value;
                OnPropertyChanged("Projects");
            }
        }

        private IList<Developer> developers;

        public string Name { get; private set; }

        public long CurrentBudget { get; private set; }

        public SoftwareCompany()
        {
            this.Projects = new List<Project>();
            this.developers = new List<Developer>();
        }

        public void AcceptNewProject(Project proj)
        {
            Projects.Add(proj);
        }

        public void HireDeveloper(Developer dev)
        {
            developers.Add(dev);
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

        public int GetNumberOfEmployees()
        {
            return developers.Count;
        }

        public int GetNumberOfProjects()
        {
            return Projects.Count;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
