using Game.DataServices;
using Game.Model;
using Game.UI.Command;
using System;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Game.UI.ViewModel
{
    public class DevelopersViewModel : ViewModelBase
    {
        private DevelopersDataService developersDataService;

        public ICommand HireDeveloperCommand { get; set; }
        public ICommand RejectDeveloperCommand { get; set; }

        public Developer offeredDeveloper;

        private GameEngine engine;

        public Developer OfferedDeveloper
        {
            get
            {
                return offeredDeveloper;
            }
            set
            {
                offeredDeveloper = value;
                offeredDeveloper.Picture.Freeze();
                OnPropertyChanged("OfferedDeveloper");
            }
        }

        public DevelopersViewModel(DevelopersDataService devDataService, GameEngine engine)
        {
            this.engine = engine;
            this.developersDataService = devDataService;
            this.OfferedDeveloper = developersDataService.GetNextDeveloper();

            LoadCommands();
        }

        private void LoadCommands()
        {
            HireDeveloperCommand = new DelegateCommand(HireDeveloper, CanHireDeveloper);
            RejectDeveloperCommand = new DelegateCommand(RejectDeveloper, CanRejectDeveloper);
        }

        private bool CanRejectDeveloper(object arg)
        {
            return true;
        }

        private void RejectDeveloper(object obj)
        {
            this.OfferedDeveloper = this.developersDataService.GetNextDeveloper();
        }

        private bool CanHireDeveloper(object arg)
        {
            return true;
        }

        private void HireDeveloper(object obj)
        {
            //this.engine.company.TryAcceptNewProject(offeredProject, this.engine.timer.GetCurrentTime());
            //this.OfferedProject = this.ProjectsDataService.GetNextProject();
        }
    }
}
