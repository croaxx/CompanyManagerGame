using Game.Model;
using System.Collections.ObjectModel;
using System;
using System.Windows.Input;
using Game.UI.Command;
using System.Linq;

namespace Game.UI.ViewModel
{
    public class DevelopersSummaryViewModel
    {
        public readonly int monthsFiringSpanTime;
        public ICommand FireDeveloperCommand { get; private set; }

        private GameEngine engine;

        public class DeveloperRepresentationViewModel : ViewModelBase
        {
            public IDeveloper Developer { get; }

            private DateTime? fireDate = null;
            public DateTime? FireDate
            {
                get { return fireDate; }
                set
                {
                    if (value != fireDate)
                   {
                        fireDate = value;
                        OnPropertyChanged("FireDate");
                    }
                }
            }
            public DeveloperRepresentationViewModel(IDeveloper dev)
            {
                Developer = dev;
                FireDate = Developer.FireDate;
            }
        }

        public ObservableCollection<DeveloperRepresentationViewModel> Developers { get; private set; } // DeveloperViewModel

        public DevelopersSummaryViewModel(GameEngine engine)
        {
            Developers = new ObservableCollection<DeveloperRepresentationViewModel>();
            this.engine = engine;
            this.engine.company.DeveloperAdded += OnDeveloperAdded;
            this.engine.company.DeveloperRemoved += OnDeveloperRemoved;
            monthsFiringSpanTime = 3;

            LoadCommands();
        }
        private void OnDeveloperRemoved(object sender, DeveloperEventArgs e)
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                Developers.Remove(Developers.Where(c => ReferenceEquals(c.Developer, e.developer)).Single());
            });
        }
        private void OnDeveloperAdded(object sender, DeveloperEventArgs e)
        {
            Developers.Add(new DeveloperRepresentationViewModel(e.developer));
        }
        private void LoadCommands()
        {
            FireDeveloperCommand = new DelegateCommand(ResignSpecifiedDeveloper, CanResignSpecifiedDeveloper);
        }
        private void ResignSpecifiedDeveloper(object obj)
        {
            DeveloperRepresentationViewModel d = obj as DeveloperRepresentationViewModel;
            int monthSpan = 3;
            var resignTime = engine.timer.GetCurrentTime().AddMonths(monthSpan);
            engine.company.SetDeveloperResignationTime(d.Developer, resignTime);
            d.FireDate = resignTime;
        }
        private bool CanResignSpecifiedDeveloper(object arg)
        {
            return true;
        }
    }
}
