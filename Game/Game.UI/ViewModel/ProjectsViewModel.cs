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
        public ObservableCollection<Project> Projects { get; private set; }
        
        public ProjectsViewModel()
        {
            Projects = new ObservableCollection<Project>();
            Messenger.Default.Register<Project>(this, OnProjectAccepted);
        }

        private void OnProjectAccepted(Project proj)
        {
            Projects.Add(proj);
        }

        public void Load()
        {
        }
    }
}
