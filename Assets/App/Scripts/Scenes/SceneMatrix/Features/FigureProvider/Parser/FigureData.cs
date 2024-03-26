using App.Scripts.Modules.Grid;
using App.Scripts.Scenes.SceneMatrix.Features.FigureProvider.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.App.Scripts.Scenes.SceneMatrix.Features.FigureProvider.Parser
{
    public class FigureData
    {
        public Vector2Int Size { get; private set; }
        public Grid<bool> Matrix { get; private set; }

        private int width;
        private int height;

        public FigureData(Vector2Int size)
        {
            width = size.x;
            height = size.y;
            Matrix = new Grid<bool>(size);
            Size = size;
        }

        public void SetTrue(int linearIndex)
        {
            int row = linearIndex / width;
            int col = linearIndex % width;
            if (row < 0 || col < 0 || row >= height || col >= width)
            {
                throw new ExceptionParseFigure($"Index out of bounds: {linearIndex}, {row}, {col}.");
            }
            Matrix[col, row] = true;
        }
    }
}
