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
        public DevelopersViewModel DevelopersViewModel { get; private set; }

        public DevelopersSummaryViewModel DevelopersSummaryViewModel { get; private set; }

        public GameEngine GameEngine { get; private set;}

        public MainViewModel()
        {
            var timer = new GameTimer();
            var booking = new BookingLogic();
            var company = new SoftwareCompany(booking);
            this.GameEngine = new GameEngine(timer, company);

            this.StatisticsViewModel = new StatisticsViewModel(GameEngine);
            this.ProjectsViewModel = new ProjectsViewModel(GameEngine);
            this.ProjectManagementViewModel = new ProjectManagementViewModel(new ProjectsDataService(), GameEngine);
            this.DevelopersViewModel = new DevelopersViewModel(new DevelopersDataService(), GameEngine);
            this.DevelopersSummaryViewModel = new DevelopersSummaryViewModel(GameEngine);
        }
    }
}
