using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MazeComponent
{
    public class Maze
    {
        private int width;
        private int height;
        private MazeGeneratorCell[,] _cellsGenerator;
        private MazeCell[,] _cells;
        private Vector2Int finishPosition;

        public int Width => width;
        public int Height => height;
        public MazeCell[,] Cells => _cells;
        public MazeGeneratorCell[,] CellsGenerator => _cellsGenerator;

        public MazeCell FinishPosition => _cells[finishPosition.x, finishPosition.y];

        public Maze(int width, int height)
        {
            this.width = width;
            this.height = height;
            _cellsGenerator = new MazeGeneratorCell[width, height];
            _cells = new MazeCell[width, height];
        }

        public void GenerateMaze()
        {
            InitializeCells();
            RemoveWallsWithBacktracking();

            for (var x = 0; x < _cellsGenerator.GetLength(0); x++)
            {
                _cellsGenerator[x, Height - 1].WallLeft = false;
            }

            for (var y = 0; y < _cellsGenerator.GetLength(1); y++)
            {
                _cellsGenerator[Width - 1, y].WallBottom = false;
            }

            int distanceFromStart = 0;

            for (int x = 0; x < _cellsGenerator.GetLength(0); x++)
            {
                for (int y = 0; y < _cellsGenerator.GetLength(1); y++)
                {
                    _cells[x, y] = new MazeCell(_cellsGenerator[x, y]);

                    if (distanceFromStart > _cellsGenerator[x, y].DistanceFromStart) continue;

                    distanceFromStart = _cellsGenerator[x, y].DistanceFromStart;
                    finishPosition = new Vector2Int(x, y);
                }
            }
        }

        public void LoadMaze(MazeGeneratorCell[] cells)
        {
            int distanceFromStart = 0;

            foreach (var cell in cells)
            {
                _cellsGenerator[cell.X, cell.Y] = cell;
                _cells[cell.X, cell.Y] = new MazeCell(cell);

                if (distanceFromStart > cell.DistanceFromStart) continue;

                distanceFromStart = cell.DistanceFromStart;
                finishPosition = new Vector2Int(cell.X, cell.Y);
            }
        }

        public bool CanMoveToNextCell(Vector2Int currentPosition, Vector2Int direction)
        {
            var nextPosition = currentPosition + direction;

            if (nextPosition.x < 0 || nextPosition.x >= Width || nextPosition.y < 0 || nextPosition.y >= Height)
            {
                return false;
            }

            switch (direction.x)
            {
                case -1:
                    return !_cells[currentPosition.x, currentPosition.y].WallLeft;
                case 1:
                    return !_cells[nextPosition.x, nextPosition.y].WallLeft;
                default:
                {
                    switch (direction.y)
                    {
                        case -1:
                            return !_cells[currentPosition.x, currentPosition.y].WallBottom;
                        case 1:
                            return !_cells[nextPosition.x, nextPosition.y].WallBottom;
                    }

                    break;
                }
            }

            return false;
        }

        public bool CheckPositionForFinish(Vector2Int posMaze)
        {
            return finishPosition == posMaze;
        }

        private void InitializeCells()
        {
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    _cellsGenerator[x, y] = new MazeGeneratorCell { X = x, Y = y };
                }
            }
        }

        private void RemoveWall(MazeGeneratorCell a, MazeGeneratorCell b)
        {
            if (a.X == b.X)
            {
                if (a.Y > b.Y) a.WallBottom = false;
                else b.WallBottom = false;
            }
            else
            {
                if (a.X > b.X) a.WallLeft = false;
                else b.WallLeft = false;
            }
        }

        private void RemoveWallsWithBacktracking()
        {
            for (int x = 0; x < _cellsGenerator.GetLength(0); x++)
            {
                for (int y = 0; y < _cellsGenerator.GetLength(1); y++)
                {
                    _cellsGenerator[x, y].Visited = false;
                }
            }

            var current = _cellsGenerator[0, 0];
            current.Visited = true;
            current.DistanceFromStart = 0;

            var stack = new Stack<MazeGeneratorCell>();
            do
            {
                var unvisitedNeighbours = GetUnvisitedNeighbours(current);

                if (unvisitedNeighbours.Count > 0)
                {
                    var chosen = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];
                    RemoveWall(current, chosen);

                    chosen.Visited = true;
                    stack.Push(chosen);
                    chosen.DistanceFromStart = current.DistanceFromStart + 1;
                    current = chosen;
                }
                else
                {
                    current = stack.Pop();
                }
            } while (stack.Count > 0);
        }

        private List<MazeGeneratorCell> GetUnvisitedNeighbours(MazeGeneratorCell cell)
        {
            var unvisitedNeighbours = new List<MazeGeneratorCell>();
            var x = cell.X;
            var y = cell.Y;

            if (x > 0 && !_cellsGenerator[x - 1, y].Visited) unvisitedNeighbours.Add(_cellsGenerator[x - 1, y]);
            if (y > 0 && !_cellsGenerator[x, y - 1].Visited) unvisitedNeighbours.Add(_cellsGenerator[x, y - 1]);
            if (x < width - 2 && !_cellsGenerator[x + 1, y].Visited) unvisitedNeighbours.Add(_cellsGenerator[x + 1, y]);
            if (y < height - 2 && !_cellsGenerator[x, y + 1].Visited) unvisitedNeighbours.Add(_cellsGenerator[x, y + 1]);

            return unvisitedNeighbours;
        }


        [Serializable]
        public class MazeGeneratorCell
        {
            public int X;
            public int Y;

            public bool WallLeft = true;
            public bool WallBottom = true;

            public bool Visited = false;
            public int DistanceFromStart;
        }
    }

    public class MazeCell
    {
        public int X;
        public int Y;

        public bool WallLeft = true;
        public bool WallBottom = true;

        public MazeCell(Maze.MazeGeneratorCell cell)
        {
            X = cell.X;
            Y = cell.Y;
            WallLeft = cell.WallLeft;
            WallBottom = cell.WallBottom;
        }
    }
}