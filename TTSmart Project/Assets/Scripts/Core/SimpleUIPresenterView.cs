using UnityEngine;

namespace Core
{
    public class SimpleUIPresenterView<TP, TV> : SimplePresenterView<TP, TV> where TP : SimpleUIPresenter<TP, TV> where TV : SimpleUIPresenterView<TP, TV>
    {
        public RectTransform RectTransform { get; private set; }

        public override void Init()
        {
            RectTransform = GetComponent<RectTransform>();
        }
    }
}