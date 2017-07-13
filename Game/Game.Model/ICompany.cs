using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Game.Model
{
    public interface ICompany
    {
        event EventHandler<EventArgs> ProjectsCollectionChange;
        event EventHandler<EventArgs> DevelopersCollectionChange;
        string GetCompanyName();
        long GetCompanyBudget();
        int GetNumberOfProjects();
        int GetNumberOfDevelopers();
        bool TryAcceptNewProject(Project project, DateTime startTime);
        bool TryHireDeveloper(Developer d);
        void QuitProject(string title);
        IList<Project> GetProjects();
        IList<Developer> GetDevelopers();
        void UpdateProjectsStatus(DateTime time);
        void FireDeveloper(Developer d);
    }
}
