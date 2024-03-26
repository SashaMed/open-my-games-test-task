using App.Scripts.Modules.Grid;
using System;
using UnityEngine;

namespace App.Scripts.Scenes.SceneMatrix.Features.FigureRotator.Services
{
    public class FigureRotatorDummy : IFigureRotator
    {
        public delegate Vector2Int RotationCalculator(int i, int j, int n, int m);


        public Grid<bool> RotateFigure(Grid<bool> grid, int rotateCount)
        {
            RotationCalculator rotationDirection = (rotateCount > 0) ? CalculateClockwise : CalculateCounterClockwise;
            var absRotationCount = Math.Abs(rotateCount % 4);
            var rotated = new Grid<bool>(grid.Size);
            rotated = grid;
            for (int i = 0; i < absRotationCount; i++)
            {
                rotated = Rotate(rotated, rotationDirection);
            }
            return rotated;
        }


        private Grid<bool> Rotate(Grid<bool> grid, RotationCalculator indexCalculator)
        {
            var newSize = new Vector2Int(grid.Size.y, grid.Size.x);
            var rotated = new Grid<bool>(newSize);
            var n = grid.Size.x;
            var m = grid.Size.y;

            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < m; ++j)
                {
                    var index = indexCalculator(i, j, n, m);
                    rotated[index.x, index.y] = grid[i, j];
                }
            }

            return rotated;
        }

        private Vector2Int CalculateClockwise(int i, int j, int n, int m)
        {
            return new Vector2Int(j, n - i - 1);
        }

        private Vector2Int CalculateCounterClockwise(int i, int j, int n, int m)
        {
            return new Vector2Int(m - j - 1, i);
        }
    }
}