using Game.DataServices;
using Game.Model;
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
            this.StatisticsViewModel = new StatisticsViewModel(new SoftwareCompany());
            this.ProjectsViewModel = new ProjectsViewModel();
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
