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
        //public ICommand IncrementTimeCommand { get; private set; }

        public GameEngine engine { get; private set; }

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
                this.engine.timer.SetTimeSpeedFactor(timeSpeedFactor);
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

        public StatisticsViewModel(GameEngine engine)
        {
            this.engine = engine;
            this.engine.timer.SetTimeSpeedFactor(TimeSpeedFactor);
            this.engine.timer.LaunchAsync();

            this.engine.timer.TimerUpdateEvent += OnTimerUpdateEvent;
            this.engine.company.ProjectsCollectionChange += (o, e) => { this.NumberOfProjects = this.engine.company.GetNumberOfProjects(); };
        }

        private void OnTimerUpdateEvent(object sender, TimerUpdateEventArgs e)
        {
            this.CurrentGameTime = e.TimerArgs;
        }
    }
}
