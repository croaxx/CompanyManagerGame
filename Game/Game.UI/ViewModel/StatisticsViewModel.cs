using Game.Model;
using Game.UI.Utility;
using System;
using System.ComponentModel;

namespace Game.UI.ViewModel
{
    public class StatisticsViewModel : ViewModelBase
    {
        public ICompany SoftwareCompany { get; private set; }

        private int numberOfProjects;

        public int NumberOfProjects
        { 
            get
            {
                return numberOfProjects;
            }

            set
            {
                numberOfProjects = value;
                OnPropertyChanged("NumberOfProjects");
            }
        }
        
        public StatisticsViewModel(ICompany company)
        {
            this.CurrentGameTime = DateTime.Now;
            this.SoftwareCompany = company;
            this.SoftwareCompany.PropertyChanged += OnCompanyPropertyChanged;
            Messenger.Default.Register<Project>(this, OnProjectAccepted);
        }

        private void OnProjectAccepted(Project proj)
        {
            SoftwareCompany.AcceptNewProject(proj);
            NumberOfProjects = SoftwareCompany.GetNumberOfProjects();
        }

        private void OnCompanyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if ( e.PropertyName == "Projects" )
            {
                NumberOfProjects = SoftwareCompany.GetNumberOfProjects();
            }
        }

        public DateTime CurrentGameTime { get; private set; }
        public void Load()
        {
            //this.CurrentGameTime = DateTime.Now; 
        }
    }
}
