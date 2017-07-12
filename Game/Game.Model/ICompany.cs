using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Game.Model
{
    public interface ICompany
    {
        event EventHandler<EventArgs> ProjectsCollectionChange;
        string GetCompanyName();
        long GetCompanyBudget();
        int GetNumberOfProjects();
        bool TryAcceptNewProject(Project project, DateTime startTime);
        void QuitProject(string title);
        IList<Project> GetProjects();
        void UpdateProjectsStatus(DateTime time);
    }
}
