using Core;
using DG.Tweening;
using Settings;
using UnityEngine;

namespace GameComponent
{
    [PrefabInfo(Enumerators.NamePrefabAddressable.Player)]
    public class PlayerPresenterView : SimplePresenterView<PlayerPresenter,PlayerPresenterView>
    {
        [SerializeField] private float moveSpeed = 0.5f;
        
        private Sequence _moveSequence;
        public override void Init()
        {
            _moveSequence = DOTween.Sequence();
        }

        public void AddMovePoint(Vector3 nextPos)
        {
            _moveSequence.Append(transform.DOMove(nextPos,moveSpeed));
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _moveSequence.Kill();
            transform.DOKill();
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }
    }
}
