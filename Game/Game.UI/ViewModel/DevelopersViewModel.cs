using Game.DataServices;
using Game.Model;
using Game.UI.Command;
using System.Windows.Input;

namespace Game.UI.ViewModel
{
    public class DevelopersViewModel : ViewModelBase
    {
        private IDeveloperDataService developersDataService;

        public ICommand HireDeveloperCommand { get; private set; }
        public ICommand RejectDeveloperCommand { get; private set; }

        private GameEngine engine;

        public IDeveloper offeredDeveloper;
        public IDeveloper OfferedDeveloper
        {
            get
            {
                return offeredDeveloper;
            }
            set
            {
                offeredDeveloper = value;
                (offeredDeveloper as Developer)?.Picture.Freeze();
                OnPropertyChanged("OfferedDeveloper");
            }
        }

        public DevelopersViewModel(DevelopersDataService devDataService, GameEngine engine)
        {
            this.engine = engine;
            developersDataService = devDataService;
            OfferedDeveloper = developersDataService.GetNextDeveloper();
            LoadCommands();
        }

        private void LoadCommands()
        {
            HireDeveloperCommand = new DelegateCommand(HireDeveloper, CanHireDeveloper);
            RejectDeveloperCommand = new DelegateCommand(RejectDeveloper, CanRejectDeveloper);
        }
        private bool CanRejectDeveloper(object arg)
        {
            return developersDataService.IsNextDeveloperAvailable();
        }
        private void RejectDeveloper(object obj)
        {
            OfferedDeveloper = developersDataService.GetNextDeveloper();
        }
        private bool CanHireDeveloper(object arg)
        {
            return OfferedDeveloper != null;
        }
        private void HireDeveloper(object obj)
        {
            if (OfferedDeveloper != null)
            {
                engine.company.TryHireDeveloper(OfferedDeveloper);

                if (developersDataService.IsNextDeveloperAvailable())
                    OfferedDeveloper = developersDataService.GetNextDeveloper();
                else
                    OfferedDeveloper = null;
            }
        }
    }
}
