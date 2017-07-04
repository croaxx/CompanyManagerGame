using Game.UI.ViewModel;
using System.Windows;
using System;

namespace Game.UI.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel viewModel;
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            this.Loaded += MainViewModel_Loaded;
            this.viewModel = viewModel;
            this.DataContext = this.viewModel;
        }
        private void MainViewModel_Loaded(object sender, RoutedEventArgs e)
        {
            this.viewModel.Load();
        }
    }
}
