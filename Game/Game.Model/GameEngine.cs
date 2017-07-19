using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Game.Model
{
    public class GameEngine
    {
        public ITimer timer;

        public ICompany company;

        public ICompanyLogic logic;

        public GameEngine(ITimer timer, ICompany company, ICompanyLogic logic)
        {
            this.timer = timer;
            this.company = company;
            this.logic = logic;
            this.timer.TimerUpdateEvent += OnTimerUpdateEvent;
        }

        private void OnTimerUpdateEvent(object sender, TimerUpdateEventArgs e)
        {
            this.company.UpdateCompanyStatus(e.TimerArgs, logic);
        }
    }
}
