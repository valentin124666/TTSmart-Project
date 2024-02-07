using System;
using Core;
using DG.Tweening;
using Managers;
using Managers.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIElements.Popup
{
    public class SettingsPopupView : MonoBehaviour, IUIPopup
    {
        private IGameDataManager _dataManager;

        [SerializeField] private Transform[] pointAnimation;
        [SerializeField] private Transform media;

        [SerializeField] private Button okButton;
        [SerializeField] private TMP_Text wightScore;
        [SerializeField] private TMP_Text heightScore;
        [SerializeField] private Slider wightSlider;
        [SerializeField] private Slider heightSlider;

        public bool IsActive => gameObject.activeSelf;

        public void Show()
        {
            gameObject.SetActive(true);
            AnimationShowPopup();
            _dataManager = GameClient.Instance.GetService<IGameDataManager>();
            var mazeData = _dataManager.GetDataOfType<MazeData>();

            wightSlider.onValueChanged.AddListener(SetWightScore);
            wightSlider.value = mazeData.width;
            
            heightSlider.onValueChanged.AddListener(SetHeightScore);
            heightSlider.value = mazeData.height;


            okButton.onClick.AddListener(AgreeToChanges);
        }

        private void AgreeToChanges()
        {
            _dataManager.GetDataOfType<MazeData>().width = Mathf.RoundToInt(wightSlider.value);
            _dataManager.GetDataOfType<MazeData>().height = Mathf.RoundToInt(heightSlider.value);

            Hide();
        }

        public void Hide()
        {
            gameObject.SetActive(false);

            Reset();
        }

        public void Reset()
        {
            okButton.onClick.RemoveAllListeners();
            wightSlider.onValueChanged.RemoveAllListeners();
            heightSlider.onValueChanged.RemoveAllListeners();
        }

        private void SetWightScore(float score)
        {
            wightScore.text = Mathf.Round(score).ToString();
        }

        private void SetHeightScore(float score)
        {
            heightScore.text = Mathf.Round(score).ToString();
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