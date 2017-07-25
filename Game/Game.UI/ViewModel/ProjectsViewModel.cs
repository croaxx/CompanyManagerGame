using Game.Model;
using System.Collections.ObjectModel;
using System;
using System.Windows.Input;
using Game.UI.Command;

namespace Game.UI.ViewModel
{
    public class ProjectsViewModel : ViewModelBase
    {
        public ICommand RemoveProjectCommand { get; private set; }
        public class ProjectRepresentationViewModel : ViewModelBase
        {
            public IProject Project { get; }

            private double _percentageTimePassed;
            public double PercentageTimePassed {
                get{return _percentageTimePassed; }
                set
                {
                    if (value != _percentageTimePassed)
                   {
                        _percentageTimePassed = value;
                        OnPropertyChanged("PercentageTimePassed");
                    }
                    
                } }
            public ProjectRepresentationViewModel(IProject proj, ITimer timer)
            {
                Project = proj;
                PercentageTimePassed = proj.GetPercentageTimePassed(timer.GetCurrentTime());
            }
        }

        public ObservableCollection<ProjectRepresentationViewModel> Projects { get; private set; }

        private GameEngine engine;

        public ProjectsViewModel(GameEngine engine)
        {
            Projects = new ObservableCollection<ProjectRepresentationViewModel>();
            this.engine = engine;
            this.engine.company.ProjectsCollectionChange += OnProjectsCollectionChange;
            LoadCommands();
        }
        private void OnProjectsCollectionChange(object sender, EventArgs e)
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                Projects.Clear();

                foreach (var p in engine.company.GetProjects())
                {
                    Projects.Add(new ProjectRepresentationViewModel(p, engine.timer));
                }
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
            engine.company.TryQuitProjectAndPunishBudget(p.Project);
            Projects.Remove(p);
        }
    }
}
