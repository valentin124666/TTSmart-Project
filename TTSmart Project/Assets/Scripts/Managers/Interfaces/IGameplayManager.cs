using Settings;

namespace Managers.Interfaces
{
    public interface IGameplayManager 
    {
        T GetController<T>() where T : IController;

        Enumerators.AppState CurrentState { get; }
        void ChangeAppState(Enumerators.AppState stateTo);

        void ExitMainMenu();
    }
}
