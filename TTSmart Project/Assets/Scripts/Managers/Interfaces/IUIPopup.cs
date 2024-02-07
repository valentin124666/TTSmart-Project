using System;

namespace Managers.Interfaces
{
    public interface IUIPopup
    {
        bool IsActive { get; }
        void Show();
        void Hide();
        void Reset();
    }
}