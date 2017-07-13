using Game.Model;
using System.Collections.ObjectModel;
using System;
using System.Windows.Input;
using Game.UI.Command;

namespace Game.UI.ViewModel
{
    public class DevelopersSummaryViewModel
    {
        public ICommand FireDeveloperCommand { get; private set; }

        private GameEngine engine;

        public ObservableCollection<Developer> Developers { get; private set; }

        public DevelopersSummaryViewModel(GameEngine engine)
        {
            this.Developers = new ObservableCollection<Developer>();
            this.engine = engine;
            this.engine.company.DevelopersCollectionChange += OnDevelopersCollectionChange;

            LoadCommands();
        }

        private void LoadCommands()
        {
            FireDeveloperCommand = new DelegateCommand(RemoveSpecifiedDeveloper, CanRemoveSpecifiedDeveloper);
        }

        private void RemoveSpecifiedDeveloper(object obj)
        {
            Developer d = obj as Developer;
            this.engine.company.FireDeveloper(d);
            Developers.Remove(d);
        }

        private bool CanRemoveSpecifiedDeveloper(object arg)
        {
            return true;
        }

        private void OnDevelopersCollectionChange(object sender, EventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                Developers.Clear();

                foreach (var d in this.engine.company.GetDevelopers())
                {
                    Developers.Add(d);
                }
            });
        }
    }
}
