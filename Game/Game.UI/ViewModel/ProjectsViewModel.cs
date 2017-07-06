using Game.Model;
using System.Collections.ObjectModel;
using Game.UI.Utility;
using System;
using System.Windows.Input;
using System.Collections.Generic;
using Game.UI.Command;

namespace Game.UI.ViewModel
{
    public class ProjectsViewModel : ViewModelBase
    {
        public ObservableCollection<Project> Projects { get; private set; }

        public ICompany company;
        
        public ProjectsViewModel(ICompany company)
        {
            Projects = new ObservableCollection<Project>();
            this.company = company;
            Messenger.Default.Register<Project>(this, OnProjectAccepted);
            LoadCommands();
        }

        public ICommand RemoveProjectCommand { get; private set; }

        private void LoadCommands()
        {
             RemoveProjectCommand= new DelegateCommand(RemoveSpecifiedProject, CanRemoveSpecifiedProject);
        }

        private bool CanRemoveSpecifiedProject(object arg)
        {
            return true;
        }

        private void RemoveSpecifiedProject(object obj)
        {
            Project p = obj as Project;
            this.company.QuitProject(p.Title);
            Projects.Remove(p);
        }

        private void OnProjectAccepted(Project proj)
        {
            Projects.Add(proj);
        }

        public void Load()
        {
        }
    }
}
