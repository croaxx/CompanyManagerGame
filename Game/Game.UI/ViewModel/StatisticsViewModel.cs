using Game.Model;
using System;

namespace Game.UI.ViewModel
{
    public class StatisticsViewModel : ViewModelBase
    {
        public GameEngine engine { get; }
        public DateTime FoundationDate { get; }

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

        private double budgetCurrent;
        public double BudgetCurrent
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

        private int timeSpeedFactor;
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
                engine.timer.SetTimeSpeedFactor(timeSpeedFactor);
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
        public StatisticsViewModel(GameEngine engine)
        {
            this.engine = engine;
            TimeSpeedFactor = 1;
            this.engine.timer.SetTimeSpeedFactor(TimeSpeedFactor);
            this.engine.timer.RunTimerAsync();

            FoundationDate = this.engine.timer.GetCurrentTime();
            BudgetCurrent = this.engine.company.GetCompanyBudget();
            NextSalaryPaymentDate = this.engine.company.GetNextSalaryPaymentDate();

            this.engine.timer.TimerUpdateEvent += OnTimerUpdateEvent;
            this.engine.company.ProjectAdded += (o, e) => { NumberOfProjects = this.engine.company.GetNumberOfProjects(); };
            this.engine.company.ProjectRemoved += (o, e) => { NumberOfProjects = this.engine.company.GetNumberOfProjects(); };
            this.engine.company.DeveloperAdded += (o, e) => { NumberOfDevelopers = this.engine.company.GetNumberOfDevelopers(); };
            this.engine.company.DeveloperRemoved += (o, e) => { NumberOfDevelopers = this.engine.company.GetNumberOfDevelopers(); };
            this.engine.company.BudgetChanged += (o, e) => { BudgetCurrent = this.engine.company.GetCompanyBudget(); };
        }
        private void OnTimerUpdateEvent(object sender, TimerUpdateEventArgs e)
        {
            NextSalaryPaymentDate = engine.company.GetNextSalaryPaymentDate();
            CurrentGameTime = e.TimerArgs;
        }
    }
}
