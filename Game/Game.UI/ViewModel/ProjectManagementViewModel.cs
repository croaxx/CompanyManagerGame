using Game.Model;
using Game.DataServices;
using System.Windows.Input;
using Game.UI.Command;

namespace Game.UI.ViewModel
{
    public class ProjectManagementViewModel : ViewModelBase
    {
        private IProjectDataService projectsDataService { get; }

        public GameEngine engine;
        
        private IProject offeredProject;
        public IProject OfferedProject
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

        public ProjectManagementViewModel(IProjectDataService dataService, GameEngine engine)
        {
            projectsDataService = dataService;
            this.engine = engine;
            OfferedProject = dataService.GetNextProject();
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
            engine.company.TryAcceptNewProject(offeredProject, engine.timer.GetCurrentTime());
            
            if (projectsDataService.IsNextProjectAvailable())
                OfferedProject = projectsDataService.GetNextProject();
        }
        private bool CanLoadNextProject(object arg)
        {
            return projectsDataService.IsNextProjectAvailable();
        }
        private void LoadNextProject(object obj)
        {
            OfferedProject = projectsDataService.GetNextProject();
        }
    }
}
