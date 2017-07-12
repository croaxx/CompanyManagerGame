using System.Collections;
using System.Collections.Generic;

namespace Game.Model
{
    public interface IBookingLogic
    {
        void BookTime(IEnumerable<Project> projects, IEnumerable<Developer> developers);
    }
}