using Core;
using Managers;
using Managers.Interfaces;
using UnityEngine;

namespace GameComponent
{
    public  class PlayerPresenter : SimplePresenter<PlayerPresenter,PlayerPresenterView>
    {
        private CellPresenter _currentCell;
        private readonly IGameDataManager _dataManager;

        public Vector2Int PositionOnMaze => _currentCell?.MazePos ?? default;
        public CellPresenter CurrentCell => _currentCell;
        
        public PlayerPresenter(PlayerPresenterView view) : base(view)
        {
            _dataManager = GameClient.Instance.GetService<IGameDataManager>();
        }

        public void SetPositionPlayer(CellPresenter cell)
        {
            _currentCell = cell;
            _dataManager.GetDataOfType<LevelData>().posPlayer = _currentCell.MazePos;
            
            View.SetPosition(cell.WorldPos);
        }

        public void MovePlayer(CellPresenter cell)
        {
            _currentCell = cell;
            _dataManager.GetDataOfType<LevelData>().posPlayer = _currentCell.MazePos;

            View.AddMovePoint(cell.WorldPos);
        }
    }
}
