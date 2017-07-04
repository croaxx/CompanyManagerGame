using System.ComponentModel;

namespace Game.Model
{
    public interface ICompany : INotifyPropertyChanged
    {
        string GetCompanyName();
        long GetCompanyBudget();
        int GetNumberOfEmployees();
        int GetNumberOfProjects();
        void AcceptNewProject(Project project);
    }
}
