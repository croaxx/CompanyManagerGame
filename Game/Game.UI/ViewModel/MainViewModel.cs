using Game.DataServices;
using Game.Model;
using Game.UI.DataProvider;

namespace Game.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public StatisticsViewModel StatisticsViewModel { get; }
        public ProjectsViewModel ProjectsViewModel { get; }
        public ProjectManagementViewModel ProjectManagementViewModel { get; }
        public DevelopersViewModel DevelopersViewModel { get; }
        public DevelopersSummaryViewModel DevelopersSummaryViewModel { get; }

        public MainViewModel()
        {
            var timer = new GameTimer();
            var company = new SoftwareCompany(timer.GetCurrentTime());
            var logic = new CompanyLogic();
            var gameEngine = new GameEngine(timer, company, logic);

            StatisticsViewModel = new StatisticsViewModel(gameEngine);
            ProjectsViewModel = new ProjectsViewModel(gameEngine);
            ProjectManagementViewModel = new ProjectManagementViewModel(new ProjectsDataService(), gameEngine);
            DevelopersViewModel = new DevelopersViewModel(new DevelopersDataService(), gameEngine);
            DevelopersSummaryViewModel = new DevelopersSummaryViewModel(gameEngine);
        }
    }
}
