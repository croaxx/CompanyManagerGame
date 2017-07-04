using Game.Model;
using System;
using System.Collections.Generic;

namespace Game.DataServices
{
    public class ProjectsDataService
    {
        private IList<Project> projects;

        private int currentProjectIdx;

        public ProjectsDataService()
        {
            this.projects = new List<Project>
            {
                { new Project("Quake IV development", new DateTime(2020, 5, 12))},
                { new Project("Client Service implementation", new DateTime(2022, 6, 11))},
                { new Project("Refactoring the code", new DateTime(2023, 7, 10))},
                { new Project("Youtube service implementation", new DateTime(2024, 8, 9))},
            };
            this.currentProjectIdx = 0;
        }
        public IEnumerable<Project> GetAllProjects()
        {
            var projects = new List<Project> 
            {
                { new Project("Bank account database", new DateTime(2020, 5, 12))},
                { new Project("Consulting for BMW", new DateTime(2020, 5, 12))},
                { new Project("Bitcoin trader", new DateTime(2020, 5, 12))},
                { new Project("Google earth", new DateTime(2020, 5, 12))}
            };

            return projects;
        }

        public Project GetNextProject()
        {
            return  projects[currentProjectIdx++];
        }

        public bool IsNextProjectAvailable()
        {
            return currentProjectIdx < projects.Count;
        }
    }
}