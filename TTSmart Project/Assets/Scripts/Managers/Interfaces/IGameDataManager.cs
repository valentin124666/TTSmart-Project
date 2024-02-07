namespace Managers.Interfaces
{
    public interface IGameDataManager
    { 
        void SaveDataClients();

        T GetDataOfType<T>() where T: SimpleData;
    }
}
