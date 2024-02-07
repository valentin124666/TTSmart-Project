using Managers.Interfaces;

namespace UIElements
{
    public class InGameplayPagePresenter : SimpleUIPresenter<InGameplayPagePresenter,InGameplayPagePresenterView>, IUIElement
    {

        public InGameplayPagePresenter(InGameplayPagePresenterView view) : base(view)
        {
        }
        
        public void SetTextNumberSteps(int number) => View.SetTextNumberSteps(number.ToString());
        public void SetTextTimer(int number) => View.SetTextTimer(number.ToString());
        
        public void Show()
        {
            SetActive(true);
        }

        public void Hide()
        {
            SetActive(false);
        }

        public void Reset()
        {
        }
    }
}
