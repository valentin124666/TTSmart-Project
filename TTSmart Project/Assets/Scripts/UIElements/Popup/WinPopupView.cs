using Core;
using DG.Tweening;
using Managers.Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace UIElements.Popup
{
    public class WinPopupView : MonoBehaviour, IUIPopup
    {
        [SerializeField] private Transform[] pointAnimation;
        [SerializeField] private Transform media;

        [SerializeField] private Button okButton;

        public bool IsActive => gameObject.activeSelf;

        public void Show()
        {
            gameObject.SetActive(true);
            AnimationShowPopup();
            okButton.onClick.AddListener(() => GameClient.Instance.GetService<IGameplayManager>().ExitMainMenu());
        }

        public void Hide()
        {
            gameObject.SetActive(false);

            Reset();
        }

        public void Reset()
        {
            okButton.onClick.RemoveAllListeners();
        }
        private Sequence AnimationShowPopup()
        {
            var seq = DOTween.Sequence();
            if (pointAnimation.Length < 3)
            {
                return seq;
            }

            media.position = pointAnimation[0].position;
            const float duration = 0.5f;
            seq.Append(media.DOMove(pointAnimation[1].position, duration / 2));
            seq.Append(media.DOMove(pointAnimation[2].position, duration / 3));

            return seq;
        }
    }
}