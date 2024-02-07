using Core;
using Settings;
using UiElements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UIElements
{
    [PrefabInfo(Enumerators.NamePrefabAddressable.InMainMenuPage)]
    public class InMainMenuPagePresenterView : SimpleUIPresenterView<InMainMenuPagePresenter, InMainMenuPagePresenterView>
    {
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button loadGameButton;
        [SerializeField] private Button settingsButton;


        public void ResetAllButtons()
        {
            newGameButton.onClick.RemoveAllListeners();
            loadGameButton.onClick.RemoveAllListeners();
            settingsButton.onClick.RemoveAllListeners();
        }

        public void AddListenerNewGameButton(UnityAction call) => newGameButton.onClick.AddListener(call);
        public void AddListenerLoadGameButton(UnityAction call) => loadGameButton.onClick.AddListener(call);
        public void AddListenerSettingsButton(UnityAction call) => settingsButton.onClick.AddListener(call);
    }
}