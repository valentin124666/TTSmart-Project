
namespace Settings
{
    public class Enumerators
    {
        public enum NamePrefabAddressable : short
        {
            Cell = 1,
            FinishFbx = 2,
            Player = 3,
            //UI

            InMainMenuPage = 100,
            InGameplayPage = 101,
            
            SettingsPopup = 150,
            WinPopup = 151,
        }

        public enum AppState
        {
            Unknown,

            MainMenu,
            InGame,
        }
    }
}