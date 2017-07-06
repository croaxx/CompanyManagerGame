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
        public GameEngine GameEngine { get; private set;}

        public MainViewModel()
        {
            var timer = new GameTimer();
            var company = new SoftwareCompany();
            this.GameEngine = new GameEngine(timer, company);

            this.StatisticsViewModel = new StatisticsViewModel(company, timer);
            this.ProjectsViewModel = new ProjectsViewModel(company);
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
