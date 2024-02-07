using Controllers;
using Core;
using Managers.Interfaces;
using MazeComponent;
using UnityEngine;

namespace GameComponent
{
    public class CellPresenter : SimplePresenter<CellPresenter, CellPresenterView>
    {
        private MazeCell _mainMazeCell;
        private LevelController _levelController;

        public Vector2Int MazePos => new(_mainMazeCell.X, _mainMazeCell.Y);
        public Vector3 WorldPos => _levelController.GetWorldPositionCell(MazePos);

        public CellPresenter(CellPresenterView view, MazeCell mazeCell, Vector3 worldPosition) : base(view)
        {
            _levelController = GameClient.Get<IGameplayManager>().GetController<LevelController>();

            _mainMazeCell = mazeCell;
            View.SetStateWall(mazeCell.WallLeft, mazeCell.WallBottom);
            View.transform.position = worldPosition;
        }
        
    }
}