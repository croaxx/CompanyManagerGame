using System;
using System.Collections.Generic;
using Game.Model;
using Game.DataServices;

namespace Game.UI.DataProvider
{
    // ToDo: Implement properly this class
    public class ProjectDataProvider : IProjectDataProvider
    {
        private ProjectsDataService dataService;

        public ProjectDataProvider()
        {

        }
        public IEnumerable<IProject> GetAllProjects()
        {
            this.dataService = new ProjectsDataService();

            return dataService.GetAllProjects();
        }
    }
}
