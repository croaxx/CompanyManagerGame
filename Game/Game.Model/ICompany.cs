using System;
using System.Collections.Generic;

namespace Game.Model
{
    public interface ICompany
    {
        event EventHandler<ProjectEventArgs> ProjectAdded;
        event EventHandler<ProjectEventArgs> ProjectRemoved;
        event EventHandler<DeveloperEventArgs> DeveloperAdded;
        event EventHandler<DeveloperEventArgs> DeveloperRemoved;
        event EventHandler<EventArgs> BudgetChanged;

        string GetCompanyName();
        double GetCompanyBudget();
        int GetNumberOfProjects();
        int GetNumberOfDevelopers();
        bool TryAcceptNewProject(IProject project, DateTime startTime);
        bool TryHireDeveloper(IDeveloper d);
        void SetProjectOngoingStatusToFalse(IProject title);
        IEnumerable<IProject> GetProjects();
        IEnumerable<IDeveloper> GetDevelopers();
        void UpdateCompanyStatus(DateTime time, ICompanyLogic logic);
        void SetDeveloperResignationTime(IDeveloper d, DateTime time);
        DateTime GetNextSalaryPaymentDate();
    }
}
