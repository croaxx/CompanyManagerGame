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
                { new Project("Quake IV development", new DateTime(2020, 5, 12), 50000, 20000)},
                { new Project("Client Service implementation", new DateTime(2022, 6, 11), 150000, 30000)},
                { new Project("Refactoring the code", new DateTime(2023, 7, 10), 250000, 40000)},
                { new Project("Youtube service implementation", new DateTime(2024, 8, 9), 350000, 50000)},
                { new Project("BBV internal project", new DateTime(2025, 5, 24), 4350000000, 60000)},
                { new Project("Bank account database", new DateTime(2021, 4, 30), 50000, 20000)},
                { new Project("Consulting for BMW", new DateTime(2022, 5, 31), 150000, 35000)},
                { new Project("Bitcoin trader", new DateTime(2023, 12, 3), 250000, 60000)},
                { new Project("Google earth", new DateTime(2024, 8, 4), 3500000, 120000)}
            };
            this.currentProjectIdx = 0;
        }
        public IEnumerable<Project> GetAllProjects()
        {
            var projects = new List<Project> 
            {
                { new Project("Bank account database", new DateTime(2020, 5, 12), 50000, 20000)},
                { new Project("Consulting for BMW", new DateTime(2020, 5, 12), 150000, 30000)},
                { new Project("Bitcoin trader", new DateTime(2020, 5, 12), 250000, 40000)},
                { new Project("Google earth", new DateTime(2020, 5, 12), 350000, 50000)}
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