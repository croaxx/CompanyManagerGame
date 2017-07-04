using System;

namespace Game.UI.ViewModel
{
    public class StatisticsViewModel : ViewModelBase
    {
        public StatisticsViewModel()
        {
            this.CurrentGameTime = DateTime.Now;
        }
        public DateTime CurrentGameTime { get; private set; }
        public void Load()
        {
            //this.CurrentGameTime = DateTime.Now; 
        }
    }
}
