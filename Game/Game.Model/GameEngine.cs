using System;

namespace Game.Model
{
    public class GameEngine
    {
        public ITimer timer;

        public ICompany company;

        public GameEngine(ITimer timer, ICompany company)
        {
            this.timer = timer;
            this.company = company;
            this.timer.TimerUpdateEvent += OnTimerUpdateEvent;
        }

        private void OnTimerUpdateEvent(object sender, TimerUpdateEventArgs e)
        {
            this.company.UpdateProjectsStatus(e.TimerArgs);
        }
    }
}
