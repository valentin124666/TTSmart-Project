using Core;
using Managers.Interfaces;
using UIElements;
using UIElements.Popup;
using UnityEngine.Events;

namespace UiElements
{
    public class InMainMenuPagePresenter : SimpleUIPresenter<InMainMenuPagePresenter, InMainMenuPagePresenterView>, IUIElement
    {
        public InMainMenuPagePresenter(InMainMenuPagePresenterView view) : base(view)
        {
            var manager = GameClient.Instance.GetService<IUIManager>();
            View.AddListenerSettingsButton(() => manager.DrawPopup<SettingsPopupView>());
        }

        public void Show()
        {
            SetActive(true);
        }

        public void Hide()
        {
            SetActive(false);
        }
        public void AddListenerNewGameButton(UnityAction call) => View.AddListenerNewGameButton(call);
        public void AddListenerLoadGameButton(UnityAction call) => View.AddListenerLoadGameButton(call);

        public void Reset()
        {
            View.ResetAllButtons();
        }
    }
}