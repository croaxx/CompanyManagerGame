using Game.Model;

namespace Game.DataServices
{
    public interface IProjectDataService
    {
        IProject GetNextProject();
        bool IsNextProjectAvailable();
    }
}
