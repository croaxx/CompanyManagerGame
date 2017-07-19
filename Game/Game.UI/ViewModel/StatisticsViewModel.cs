using Game.Model;
using System;

namespace Game.UI.ViewModel
{
    public class StatisticsViewModel : ViewModelBase
    {
        public GameEngine engine { get; private set; }
        
        public DateTime FoundationDate { get; private set;}

        private int timeSpeedFactor = 1;

        public DateTime nextSalaryPaymentDate;

        public DateTime NextSalaryPaymentDate
        {
            get
            {
                return nextSalaryPaymentDate;
            }

            set
            {
                nextSalaryPaymentDate = value;
                OnPropertyChanged("NextSalaryPaymentDate");
            }
        }

        private long budgetCurrent;

        public long BudgetCurrent
        { 
            get
            {
                return budgetCurrent;
            }

            set
            {
                budgetCurrent = value;
                OnPropertyChanged("BudgetCurrent");
            }
        }

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
        
        private int numberOfDevelopers;

        public int NumberOfDevelopers
        { 
            get
            {
                return numberOfDevelopers;
            }
            set
            {
                numberOfDevelopers = value;
                OnPropertyChanged("NumberOfDevelopers");
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

        public StatisticsViewModel(GameEngine engine, ITimer timer)
        {
            this.engine = engine;
            this.engine.timer.SetTimeSpeedFactor(TimeSpeedFactor);
            this.engine.timer.LaunchAsync();

            this.FoundationDate = this.engine.timer.GetCurrentTime();
            this.BudgetCurrent = this.engine.company.GetCompanyBudget();
            this.NextSalaryPaymentDate = this.engine.company.GetNextSalaryPaymentDate();

            this.engine.timer.TimerUpdateEvent += OnTimerUpdateEvent;
            this.engine.company.ProjectsCollectionChange += (o, e) => { this.NumberOfProjects = this.engine.company.GetNumberOfProjects(); };
            this.engine.company.DevelopersCollectionChange += (o, e) => { this.NumberOfDevelopers = this.engine.company.GetNumberOfDevelopers(); };
            this.engine.company.BudgetChange += (o, e) => 
            { 
                this.NextSalaryPaymentDate = this.engine.company.GetNextSalaryPaymentDate();
                this.BudgetCurrent = this.engine.company.GetCompanyBudget();
            };
        }

        private void OnTimerUpdateEvent(object sender, TimerUpdateEventArgs e)
        {
            this.CurrentGameTime = e.TimerArgs;
        }
    }
}
