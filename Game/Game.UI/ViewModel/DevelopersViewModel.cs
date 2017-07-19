using Game.DataServices;
using Game.Model;
using Game.UI.Command;
using System.Windows.Input;

namespace Game.UI.ViewModel
{
    public class DevelopersViewModel : ViewModelBase
    {
        private DevelopersDataService developersDataService;

        public ICommand HireDeveloperCommand { get; private set; }
        public ICommand RejectDeveloperCommand { get; private set; }

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
                offeredDeveloper?.Picture.Freeze();
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
            return this.developersDataService.IsNextDeveloperAvailable();
        }

        private void RejectDeveloper(object obj)
        {
            this.OfferedDeveloper = this.developersDataService.GetNextDeveloper();
        }

        private bool CanHireDeveloper(object arg)
        {
            return this.OfferedDeveloper != null;
        }

        private void HireDeveloper(object obj)
        {
            if (this.OfferedDeveloper != null)
            {
                this.engine.company.TryHireDeveloper(this.OfferedDeveloper);

                if (this.developersDataService.IsNextDeveloperAvailable())
                    this.OfferedDeveloper = this.developersDataService.GetNextDeveloper();
                else
                    this.OfferedDeveloper = null;
            }
        }
    }
}
