using Game.DataServices;
using Game.UI.DataProvider;

namespace Game.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public StatisticsViewModel StatisticsViewModel { get; private set; }
        public ProjectsViewModel ProjectsViewModel { set; private get; }
        public ProjectManagementViewModel ProjectManagementViewModel { set; private get; }

        public MainViewModel()
        {
            this.StatisticsViewModel = new StatisticsViewModel();
            this.ProjectsViewModel = new ProjectsViewModel(new ProjectDataProvider());
            this.ProjectManagementViewModel = new ProjectManagementViewModel(new ProjectsDataService());
        }

        public void Load()
        {
            this.StatisticsViewModel.Load();
            this.ProjectsViewModel.Load();
            this.ProjectManagementViewModel.Load();
        }
    }
}
