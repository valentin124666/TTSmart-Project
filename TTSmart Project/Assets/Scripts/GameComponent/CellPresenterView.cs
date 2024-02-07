using Core;
using Settings;
using UnityEngine;

namespace GameComponent
{
    [PrefabInfo(Enumerators.NamePrefabAddressable.Cell)]
    public class CellPresenterView : SimplePresenterView<CellPresenter,CellPresenterView>
    {
        [SerializeField] private GameObject wallLeft;
        [SerializeField] private GameObject wallBottom;
        
        public override void Init()
        {
            wallLeft.SetActive(false);
            wallBottom.SetActive(false);

        }

        public void SetStateWall(bool left,bool bottom)
        {
            wallLeft.SetActive(left);
            wallBottom.SetActive(bottom);
        }
    }
}
