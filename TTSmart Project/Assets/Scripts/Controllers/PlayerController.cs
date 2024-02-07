using Core;
using GameComponent;
using Managers;
using Managers.Interfaces;
using UIElements;
using UIElements.Popup;
using UnityEngine;

namespace Controllers
{
    public class PlayerController : IController
    {
        private Transform _parentlessPoolPlayer;
        private PlayerPresenter _playerPresenter;
        private LevelController _levelController;
        private IGameDataManager _dataManager;
        private IUIManager _uiManager;

        public bool IsInit { get; private set; }

        private int _stepsTaken;

        public void Init()
        {
            _parentlessPoolPlayer = new GameObject().transform;
            _parentlessPoolPlayer.name = "[ParentlessPoolPlayer]";
            
            _levelController = GameClient.Get<IGameplayManager>().GetController<LevelController>();
            _dataManager = GameClient.Instance.GetService<IGameDataManager>();
            _uiManager = GameClient.Instance.GetService<IUIManager>();
        }

        public void GenerationPlayer()
        {
            _playerPresenter = ResourceLoader.Instantiate<PlayerPresenter, PlayerPresenterView>(_parentlessPoolPlayer, "");
            MainApp.Instance.UpdateEvent += CheckInputDown;
        }

        public void DestroyPlayer()
        {
            _playerPresenter.Destroy();
            _playerPresenter = null;
        }

        public void PutPlayerStartingPosition()
        {
            _stepsTaken = 0;

            _playerPresenter?.SetPositionPlayer(_levelController.GetPositionFirstCell());
        }

        public void PutPlayerLoadPositionMaze(Vector2Int mazePos, int stepsTaken)
        {
            _stepsTaken = stepsTaken;

            _playerPresenter?.SetPositionPlayer(_levelController.GetCellForPos(mazePos));
            _uiManager.GetPage<InGameplayPagePresenter>().SetTextNumberSteps(_stepsTaken);
        }

        private void CheckInputDown()
        {
            Vector2Int direction = default;

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                direction.y = 1;
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                direction.y = -1;
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                direction.x = -1;
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                direction.x = 1;
            }

            if (direction == default) return;

            if (!_levelController.CanMoveToNextCell(_playerPresenter.CurrentCell, direction)) return;
            
            _stepsTaken++;
            _dataManager.GetDataOfType<LevelData>().stepsTaken = _stepsTaken;
            _uiManager.GetPage<InGameplayPagePresenter>().SetTextNumberSteps(_stepsTaken);
            var cell = _levelController.GetCellForPos(_playerPresenter.PositionOnMaze + direction);
            _playerPresenter.MovePlayer(cell);

            if (!_levelController.CheckFinish(cell)) return;
            
            MainApp.Instance.UpdateEvent -= CheckInputDown;
            _uiManager.DrawPopup<WinPopupView>();
        }
    }
}