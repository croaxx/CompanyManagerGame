using Game.Model;
using System.Collections.ObjectModel;
using Game.DataServices;
using Game.UI.DataProvider;
using Game.UI.Utility;
using System;

namespace Game.UI.ViewModel
{
    public class ProjectsViewModel : ViewModelBase
    {
        private IProjectDataProvider dataProvider;
        public ObservableCollection<Project> Projects { get; private set; }
        public ProjectsViewModel(IProjectDataProvider dataProvider)
        {
            Projects = new ObservableCollection<Project>();
            this.dataProvider = dataProvider;
            Messenger.Default.Register<Project>(this, OnProjectAccepted);
        }

        private void OnProjectAccepted(Project proj)
        {
            Projects.Add(proj);
        }

        public void Load()
        {
            Projects.Clear();
            foreach (var project in dataProvider.GetAllProjects())
            {
                Projects.Add(project);
            }
        }
    }
}
