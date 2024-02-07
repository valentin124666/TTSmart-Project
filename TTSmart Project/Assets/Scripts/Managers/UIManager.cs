using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Interfaces;
using Managers.Interfaces;
using Settings;
using UiElements;
using UIElements;
using UIElements.Popup;
using UnityEngine;

namespace Managers
{
    public class UIManager : IService, IUIManager
    {
        private List<IUIElement> _uiPages;
        private List<IUIPopup> _uiPopups;

        private GameObject _canvas { get; set; }
        public IUIElement CurrentPage { get; private set; }

        public void Init()
        {
            _canvas = MainApp.Instance.Canvas;

            _uiPages = new List<IUIElement>();

            _uiPopups = new List<IUIPopup>();
            CreateUI();

            HideAllPopups();
            HideAllPages();
        }

        private void CreateUI()
        {
            _uiPages.Add(ResourceLoader.Instantiate<InMainMenuPagePresenter,InMainMenuPagePresenterView>(_canvas.transform,""));
            _uiPages.Add(ResourceLoader.Instantiate<InGameplayPagePresenter,InGameplayPagePresenterView>(_canvas.transform,""));
            
            _uiPopups.Add(ResourceLoader.Instantiate<SettingsPopupView>(Enumerators.NamePrefabAddressable.SettingsPopup,_canvas.transform));
            _uiPopups.Add(ResourceLoader.Instantiate<WinPopupView>(Enumerators.NamePrefabAddressable.WinPopup,_canvas.transform));
        }

        public void ResetAll()
        {
            foreach (var page in _uiPages)
                page.Reset();

            foreach (var popup in _uiPopups)
                popup.Reset();
        }

        public T GetPage<T>() where T : IUIElement
        {
            IUIElement page = _uiPages.OfType<T>().First();

            return (T)page;
        }

        public T GetPopup<T>() where T : IUIPopup
        {
            IUIPopup popup =  _uiPopups.OfType<T>().First();

            return (T)popup;
        }

        public void SetPage<T>(bool hideAll = false) where T : IUIElement
        {
            if (hideAll)
            {
                HideAllPages();
            }
            else
            {
                CurrentPage?.Hide();
            }

            CurrentPage = _uiPages.OfType<T>().First();

            CurrentPage.Show();
        }

        public void DrawPopup<T>() where T : IUIPopup
        {
            IUIPopup popup = _uiPopups.OfType<T>().First();

            popup.Show();
        }

        public void HidePopup<T>() where T : IUIPopup
        {
            _uiPopups.OfType<T>().First().Hide();
        }

        public void HideAllPages()
        {
            foreach (var _page in _uiPages)
            {
                _page.Hide();
            }
        }

        public void HideAllPopups()
        {
            foreach (var _popup in _uiPopups)
            {
                _popup.Hide();
            }
        }
    }
}