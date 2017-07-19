using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

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
        bool TryAcceptNewProject(Project project, DateTime startTime);
        bool TryHireDeveloper(Developer d);
        void QuitProject(string title);
        IList<Project> GetProjects();
        IList<Developer> GetDevelopers();
        void UpdateCompanyStatus(DateTime time, ICompanyLogic logic);
        void ResignDeveloperAfterMonths(Developer d, DateTime currentime, int monthsspan);
        DateTime GetNextSalaryPaymentDate();
    }
}
