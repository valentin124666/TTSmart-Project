using Core;
using Settings;
using TMPro;
using UnityEngine;

namespace UIElements
{
    [PrefabInfo(Enumerators.NamePrefabAddressable.InGameplayPage)]
    public class InGameplayPagePresenterView : SimpleUIPresenterView<InGameplayPagePresenter, InGameplayPagePresenterView>
    {
        [SerializeField] private TMP_Text numberSteps;
        [SerializeField] private TMP_Text timer;

        public void SetTextNumberSteps(string number) => numberSteps.SetText(number);
        public void SetTextTimer(string number) => timer.SetText(number);
    }
}