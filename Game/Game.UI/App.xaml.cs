using Game.UI.View;
using Game.UI.ViewModel;
using System.Windows;

namespace Game.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainWindow = new MainWindow(new MainViewModel());

            mainWindow.Show();
        }
    }
}
