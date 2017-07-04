using Game.Model;
using Game.DataServices;
using System.Windows.Input;
using Game.UI.Command;
using Game.UI.Utility;

namespace Game.UI.ViewModel
{
    public class ProjectManagementViewModel : ViewModelBase
    {
        public ProjectsDataService ProjectsDataService { get; private set; }
        
        private Project offeredProject;
        public Project OfferedProject
        { 
            get
            {
                return offeredProject;
            }

            set
            {
                offeredProject = value;
                OnPropertyChanged("OfferedProject");
            }
        }

        public ICommand DeclineProjectCommand { get; set; }

        public ICommand AcceptProjectCommand { get; set; }

        public ProjectManagementViewModel(ProjectsDataService dataService)
        {
            this.ProjectsDataService = dataService;
            this.OfferedProject = dataService.GetNextProject();
            LoadCommands();
        }

        private void LoadCommands()
        {
            DeclineProjectCommand = new DelegateCommand(LoadNextProject, CanLoadNextProject);
            AcceptProjectCommand = new DelegateCommand(AcceptProject, CanAcceptProject);
        }

        private bool CanAcceptProject(object arg)
        {
            return OfferedProject != null;
        }

        private void AcceptProject(object obj)
        {
            Messenger.Default.Send<Project>(OfferedProject);
            this.OfferedProject = this.ProjectsDataService.GetNextProject();
        }

        private bool CanLoadNextProject(object arg)
        {
            return ProjectsDataService.IsNextProjectAvailable();
        }

        private void LoadNextProject(object obj)
        {
            this.OfferedProject = this.ProjectsDataService.GetNextProject();
        }

        public void Load()
        {
            //var dataService = new ProjectsDataService();
            //this.offeredProject = dataService.GetNextProject();
        }
    }
}
