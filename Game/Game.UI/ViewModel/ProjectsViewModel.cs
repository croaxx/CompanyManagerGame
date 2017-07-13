using Game.Model;
using System.Collections.ObjectModel;
using System;
using System.Windows.Input;
using Game.UI.Command;

namespace Game.UI.ViewModel
{
    public class ProjectsViewModel : ViewModelBase
    {
        public ObservableCollection<Project> Projects { get; private set; }

        public GameEngine engine;

        public ProjectsViewModel(GameEngine engine)
        {
            Projects = new ObservableCollection<Project>();
            this.engine = engine;
            this.engine.company.ProjectsCollectionChange += OnProjectsCollectionChange;
            LoadCommands();
        }

        private void OnProjectsCollectionChange(object sender, EventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                Projects.Clear();

                foreach (var p in this.engine.company.GetProjects())
                {
                    Projects.Add(p);
                }
            });
        }

        public ICommand RemoveProjectCommand { get; private set; }

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
            Project p = obj as Project;
            this.engine.company.QuitProject(p.Title);
            Projects.Remove(p);
        }
    }
}
