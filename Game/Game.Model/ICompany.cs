using System;
using System.Collections.Generic;

namespace Game.Model
{
    public interface ICompany
    {
        event EventHandler<EventArgs> ProjectsCollectionChange;
        event EventHandler<EventArgs> DevelopersCollectionChange;
        event EventHandler<EventArgs> BudgetChange;

        string GetCompanyName();
        long GetCompanyBudget();
        int GetNumberOfProjects();
        int GetNumberOfDevelopers();
        bool TryAcceptNewProject(IProject project, DateTime startTime);
        bool TryHireDeveloper(IDeveloper d);
        bool TryQuitProjectAndPunishBudget(IProject title);
        IEnumerable<IProject> GetProjects();
        IEnumerable<IDeveloper> GetDevelopers();
        void UpdateCompanyStatus(DateTime time, ICompanyLogic logic);
        void SetDeveloperResignationDateAfterMonths(IDeveloper d, DateTime currentime, int monthsspan);
        DateTime GetNextSalaryPaymentDate();
    }
}
