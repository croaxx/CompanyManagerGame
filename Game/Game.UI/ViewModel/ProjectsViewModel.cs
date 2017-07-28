using Game.Model;
using System.Collections.ObjectModel;
using System;
using System.Windows.Input;
using Game.UI.Command;
using System.Linq;

namespace Game.UI.ViewModel
{
    public class ProjectsViewModel : ViewModelBase
    {
        public ICommand RemoveProjectCommand { get; private set; }
        public class ProjectRepresentationViewModel : ViewModelBase
        {
            public IProject Project { get; }
            public bool ongoingStatus;
            public bool OngoingStatus
            {
                get { return ongoingStatus; }
                set
                {
                    if (value != ongoingStatus)
                   {
                        ongoingStatus = value;
                        OnPropertyChanged("OngoingStatus");
                    }
                }
            }

            private double percentageWorkCompleted;
            public double PercentageWorkCompleted
            {
                get { return percentageWorkCompleted; }
                set
                {
                    if (value != percentageWorkCompleted)
                   {
                        percentageWorkCompleted = value;
                        OnPropertyChanged("PercentageWorkCompleted");
                    }
                }
            }

            private double percentageTimePassed;
            public double PercentageTimePassed
            {
                get {return percentageTimePassed; }
                set
                {
                    if (value != percentageTimePassed)
                   {
                        percentageTimePassed = value;
                        OnPropertyChanged("PercentageTimePassed");
                    }
                }
            }
            public ProjectRepresentationViewModel(IProject proj, ITimer timer)
            {
                Project = proj;
                PercentageTimePassed = proj.GetPercentageTimePassed(timer.GetCurrentTime());
                OngoingStatus = proj.IsOngoing;
            }
        }

        public ObservableCollection<ProjectRepresentationViewModel> Projects { get; private set; }

        private GameEngine engine;

        public ProjectsViewModel(GameEngine engine)
        {
            Projects = new ObservableCollection<ProjectRepresentationViewModel>();
            this.engine = engine;
            this.engine.company.ProjectAdded += OnProjectsAdded;
            this.engine.company.ProjectRemoved += OnProjectRemoved;
            this.engine.timer.TimerUpdateEvent += OnTimerUpdateEvent;
            LoadCommands();
        }

        private void OnTimerUpdateEvent(object sender, TimerUpdateEventArgs e)
        {
            foreach (var p in Projects)
            {
                p.PercentageTimePassed = p.Project.GetPercentageTimePassed(e.TimerArgs);
                p.PercentageWorkCompleted = p.Project.WorkCompletionPercentage;
            }
        }
        private void OnProjectsAdded(object sender, ProjectEventArgs e)
        {
            Projects.Add(new ProjectRepresentationViewModel(e.project, engine.timer));
        }
        private void OnProjectRemoved(object sender, ProjectEventArgs e)
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                Projects.Remove(Projects.Where(c => ReferenceEquals(c.Project, e.project)).Single());
            });
        }
        private void LoadCommands()
        {
             RemoveProjectCommand= new DelegateCommand(RemoveSpecifiedProject, CanRemoveSpecifiedProject);
        }
        private bool CanRemoveSpecifiedProject(object arg)
        {
            return true;
        }
        private void RemoveSpecifiedProject(object obj)
        {
            ProjectRepresentationViewModel p = obj as ProjectRepresentationViewModel;
            engine.company.SetProjectOngoingStatusToFalse(p.Project);
            p.OngoingStatus = false;
        }
    }
}
