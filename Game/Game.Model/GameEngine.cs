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
        }
    }
}
