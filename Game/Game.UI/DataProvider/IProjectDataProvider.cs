using Game.Model;
using System.Collections.Generic;

namespace Game.UI.DataProvider
{
    public interface IProjectDataProvider
    {
        IEnumerable<Project> GetAllProjects();
    }
}
