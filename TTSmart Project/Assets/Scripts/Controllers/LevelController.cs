using System.Collections.Generic;
using System.Linq;
using Core;
using GameComponent;
using Managers;
using Managers.Interfaces;
using MazeComponent;
using Settings;
using UIElements;
using UnityEngine;

namespace Controllers
{
    public class LevelController : IController
    {
        private IGameDataManager _dataManager;
        private IUIManager _uiManager;

        private ParticleSystem _finishFlag;
        private Vector3 _cellOffset = new(1, 1, 0);
        private Maze _maze;
        private List<CellPresenter> _cellPresenters;
        private Transform _parentlessPoolCell;
        private float _timeInMaze;
        private int _timeInMazeToInt;

        public bool IsInit { get; private set; }

        public void Init()
        {
            _parentlessPoolCell = new GameObject().transform;
            _parentlessPoolCell.name = "[ParentlessPoolCell]";

            _dataManager = GameClient.Instance.GetService<IGameDataManager>();
            _uiManager = GameClient.Instance.GetService<IUIManager>();

            _finishFlag = ResourceLoader.Instantiate<ParticleSystem>(Enumerators.NamePrefabAddressable.FinishFbx, _parentlessPoolCell);
            _finishFlag.Stop();
            _finishFlag.gameObject.SetActive(false);

            _timeInMaze = 0;
            _timeInMazeToInt = 0;

            IsInit = true;
        }

        public void GenerationLevel()
        {
            var mazeData = _dataManager.GetDataOfType<MazeData>();
            var levelData = _dataManager.GetDataOfType<LevelData>();

            _maze = new Maze(mazeData.width, mazeData.height);
            _maze.GenerateMaze();

            levelData.width = mazeData.width;
            levelData.height = mazeData.height;

            _cellPresenters = new List<CellPresenter>();
            for (int x = 0; x < _maze.Cells.GetLength(0); x++)
            {
                for (int y = 0; y < _maze.Cells.GetLength(1); y++)
                {
                    var positionCell = GetWorldPositionCell(x, y) - (_cellOffset / 2);
                    _cellPresenters.Add(ResourceLoader.Instantiate<CellPresenter, CellPresenterView>
                        (_parentlessPoolCell, "", _maze.Cells[x, y], positionCell));
                }
            }

            levelData.cells = _maze.CellsGenerator.Cast<Maze.MazeGeneratorCell>().ToArray();
            StartLevelplay();
        }

        public void LoadLevel(int width, int height, Maze.MazeGeneratorCell[] cells)
        {
            _maze = new Maze(width, height);
            _maze.LoadMaze(cells);

            _cellPresenters = new List<CellPresenter>();
            
            for (int x = 0; x < _maze.Cells.GetLength(0); x++)
            {
                for (int y = 0; y < _maze.Cells.GetLength(1); y++)
                {
                    var positionCell = GetWorldPositionCell(x, y) - (_cellOffset / 2);
                    _cellPresenters.Add(ResourceLoader.Instantiate<CellPresenter, CellPresenterView>
                        (_parentlessPoolCell, "", _maze.Cells[x, y], positionCell));
                }
            }

            _timeInMaze = _dataManager.GetDataOfType<LevelData>().time;
            _timeInMazeToInt = (int)_timeInMaze;
            _uiManager.GetPage<InGameplayPagePresenter>().SetTextTimer(_timeInMazeToInt);

            StartLevelplay();
        }

        public CellPresenter GetPositionFirstCell()
        {
            return _cellPresenters.Find(cell => cell.MazePos == Vector2Int.zero);
        }

        public bool CanMoveToNextCell(CellPresenter currentCell, Vector2Int direction) => _maze.CanMoveToNextCell(currentCell.MazePos, direction);

        public Vector3 GetWorldPositionCell(int x, int y)
        {
            return new Vector3(x * _cellOffset.x, y * _cellOffset.y, 0) + (_cellOffset / 2);
        }

        public Vector3 GetWorldPositionCell(Vector2Int posCell)
        {
            return new Vector3(posCell.x * _cellOffset.x, posCell.y * _cellOffset.y, 0) + (_cellOffset / 2);
        }

        public Vector3 GetCenterWorldMaze()
        {
            return new Vector3((_maze.Width - 1) / 2f * _cellOffset.x, (_maze.Height - 1) / 2f * _cellOffset.y, 0);
        }

        public Bounds CalculateBoundsMaze()
        {
            Bounds bounds = new Bounds(GetCenterWorldMaze(), Vector3.zero);

            foreach (var cell in _cellPresenters)
            {
                bounds.Encapsulate(cell.WorldPos);
            }

            return bounds;
        }

        public CellPresenter GetCellForPos(Vector2Int PosCell)
        {
            return _cellPresenters.First(cell => cell.MazePos == PosCell);
        }

        public void ResetLevel()
        {
            foreach (var cell in _cellPresenters)
            {
                cell.Destroy();
            }

            _timeInMaze = 0;
            _timeInMazeToInt = 0;
            _dataManager.GetDataOfType<LevelData>().cells = null;

            _cellPresenters.Clear();
            _finishFlag.Stop();
            _finishFlag.gameObject.SetActive(false);
        }

        public bool CheckFinish(CellPresenter cell)
        {
            return _maze.CheckPositionForFinish(cell.MazePos);
        }

        private void StartLevelplay()
        {
            _finishFlag.transform.position = GetWorldPositionCell(_maze.FinishPosition.X, _maze.FinishPosition.Y);
            _finishFlag.Play();
            _finishFlag.gameObject.SetActive(true);

            MainApp.Instance.UpdateEvent += UpdateTimeMaze;
        }
        
        private void UpdateTimeMaze()
        {
            _timeInMaze += Time.deltaTime;

            if (_timeInMazeToInt<Mathf.FloorToInt(_timeInMaze))
            {
                _timeInMazeToInt = Mathf.FloorToInt(_timeInMaze);
                _dataManager.GetDataOfType<LevelData>().time = _timeInMazeToInt;
                _uiManager.GetPage<InGameplayPagePresenter>().SetTextTimer(_timeInMazeToInt);
            }
        }
    }
}