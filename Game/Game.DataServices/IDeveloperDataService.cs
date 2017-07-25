using Game.Model;

namespace Game.DataServices
{
    public interface IDeveloperDataService
    {
        IDeveloper GetNextDeveloper();
        bool IsNextDeveloperAvailable();
    }
}
