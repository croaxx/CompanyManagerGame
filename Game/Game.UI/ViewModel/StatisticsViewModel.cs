using Game.DataServices;
using Game.Model;
using Game.UI.Command;
using Game.UI.Utility;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Game.UI.ViewModel
{
    public class StatisticsViewModel : ViewModelBase
    {
        public ICommand IncrementTimeCommand { get; private set; }
        public ICompany SoftwareCompany { get; private set; }
        
        private ITimer timer;

        private int timeSpeedFactor = 1;

        public int TimeSpeedFactor 
        {
            get
            {
                return timeSpeedFactor;
            }
            set
            {
                timeSpeedFactor = value;
                OnPropertyChanged("TimeSpeedFactor");
                this.timer.SetTimeSpeedFactor(timeSpeedFactor);
            }
        }

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
        
        private DateTime currentGameTime;
        public DateTime CurrentGameTime
        {
            get
            {
                return currentGameTime;
            }

            set
            {
                currentGameTime = value;
                OnPropertyChanged("CurrentGameTime");
            }
        }

        public StatisticsViewModel(ICompany company, ITimer timer)
        {
            this.timer = timer;
            this.timer.SetTimeSpeedFactor(TimeSpeedFactor);
            this.timer.TimerUpdateEvent += OnTimerUpdateEvent;
            this.timer.LaunchAsync();

            this.SoftwareCompany = company;
            this.SoftwareCompany.ProjectsCollectionChange += (o, e) => { this.NumberOfProjects = this.SoftwareCompany.GetNumberOfProjects(); };
            Messenger.Default.Register<Project>(this, OnProjectAccepted);
            LoadCommands();
        }

        private void OnTimerUpdateEvent(object sender, TimerUpdateEventArgs e)
        {
            this.CurrentGameTime = e.TimerArgs;
        }

        private void LoadCommands()
        {
            //IncrementTimeCommand = new DelegateCommand(IncrementTime, CanIncrementTime);
        }

        private bool CanIncrementTime(object arg)
        {
            return true;
        }

        private void OnProjectAccepted(Project proj)
        {
            SoftwareCompany.TryAcceptNewProject(proj, CurrentGameTime);
            NumberOfProjects = SoftwareCompany.GetNumberOfProjects();
        }

        public void Load()
        {
            //this.CurrentGameTime = DateTime.Now; 
        }
    }
}
