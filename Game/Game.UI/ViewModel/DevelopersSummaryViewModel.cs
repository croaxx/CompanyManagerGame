using Game.Model;
using System.Collections.ObjectModel;
using System;
using System.Windows.Input;
using Game.UI.Command;

namespace Game.UI.ViewModel
{
    public class DevelopersSummaryViewModel
    {
        public readonly int monthsFiringSpanTime;
        public ICommand FireDeveloperCommand { get; private set; }

        private GameEngine engine;

        public ObservableCollection<IDeveloper> Developers { get; private set; } // DeveloperViewModel

        public DevelopersSummaryViewModel(GameEngine engine)
        {
            Developers = new ObservableCollection<IDeveloper>();
            this.engine = engine;
            this.engine.company.DevelopersCollectionChange += OnDevelopersCollectionChange;
            monthsFiringSpanTime = 3;

            LoadCommands();
        }

        private void LoadCommands()
        {
            FireDeveloperCommand = new DelegateCommand(ResignSpecifiedDeveloper, CanResignSpecifiedDeveloper);
        }
        private void ResignSpecifiedDeveloper(object obj)
        {
            Developer d = obj as Developer;
            engine.company.SetDeveloperResignationDateAfterMonths(d, engine.timer.GetCurrentTime(), monthsFiringSpanTime);
        }
        private bool CanResignSpecifiedDeveloper(object arg)
        {
            return true;
        }
        private void OnDevelopersCollectionChange(object sender, EventArgs e)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                Developers.Clear();

                foreach (var d in engine.company.GetDevelopers())
                {
                    Developers.Add(d);
                }
            });
        }
    }
}
