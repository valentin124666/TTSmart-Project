using System.Collections.Generic;
using System.Linq;
using Controllers;
using Core;
using Core.Interfaces;
using Managers.Interfaces;
using Settings;
using UiElements;
using UIElements;

namespace Managers
{
    public class GameplayManager : IService, IGameplayManager
    {
        private IUIManager _uIManager;
        private List<IController> _controllers;
        private IGameDataManager _dataManager;
        public Enumerators.AppState CurrentState { get; private set; }

        public void Init()
        {
            _uIManager = GameClient.Instance.GetService<IUIManager>();
            _dataManager = GameClient.Instance.GetService<IGameDataManager>();

            _uIManager.GetPage<InMainMenuPagePresenter>().AddListenerNewGameButton(StartNewGame);
            _uIManager.GetPage<InMainMenuPagePresenter>().AddListenerLoadGameButton(LoadGame);


            FillControllers();
        }

        public T GetController<T>() where T : IController
        {
            return (T)_controllers.Find(controller => controller is T);
        }

        private void StartNewGame()
        {
            GetController<LevelController>().GenerationLevel();
            var playerController = GetController<PlayerController>();
            playerController.GenerationPlayer();
            playerController.PutPlayerStartingPosition();
            GetController<CameraController>().InsertCameraIntoLabyrinth();

            ChangeAppState(Enumerators.AppState.InGame);
        }

        public void ExitMainMenu()
        {
            ChangeAppState(Enumerators.AppState.MainMenu);
            GetController<LevelController>().ResetLevel();
            GetController<PlayerController>().DestroyPlayer();
        }

        public void ChangeAppState(Enumerators.AppState stateTo)
        {
            switch (stateTo)
            {
                case Enumerators.AppState.MainMenu:
                    _uIManager.HideAllPopups();
                    _uIManager.SetPage<InMainMenuPagePresenter>();
                    break;
                case Enumerators.AppState.InGame:
                    _uIManager.HideAllPopups();
                    _uIManager.SetPage<InGameplayPagePresenter>();
                    break;
            }

            CurrentState = stateTo;
        }

        private void LoadGame()
        {
            var levelData = _dataManager.GetDataOfType<LevelData>();
            if (levelData.cells == null || !levelData.cells.Any())
            {
                return;
            }

            GetController<LevelController>().LoadLevel(levelData.width, levelData.height, levelData.cells);

            var playerController = GetController<PlayerController>();
            playerController.GenerationPlayer();
            playerController.PutPlayerLoadPositionMaze(levelData.posPlayer, levelData.stepsTaken);

            GetController<CameraController>().InsertCameraIntoLabyrinth();
            ChangeAppState(Enumerators.AppState.InGame);
        }

        private void FillControllers()
        {
            _controllers = new List<IController>()
            {
                new LevelController(),
                new PlayerController(),
                new CameraController()
            };

            foreach (var item in _controllers)
                item.Init();
        }
    }
}