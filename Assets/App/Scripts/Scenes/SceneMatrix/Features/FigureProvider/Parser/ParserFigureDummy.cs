using App.Scripts.Modules.Grid;
using Assets.App.Scripts.Scenes.SceneMatrix.Features.FigureProvider.Parser;
using System;
using UnityEngine;

namespace App.Scripts.Scenes.SceneMatrix.Features.FigureProvider.Parser
{


    public class ParserFigureDummy : IFigureParser
    {
        private const int matrixMaxSize = 3;

        public Grid<bool> ParseFile(string text)
        {
            var lines = text.Split('\n');
            //if (lines.Length < matrixMaxSize)
            //{
            //    throw new ExceptionParseFigure("Not enough data to parse the figure.");
            //}

            if (!int.TryParse(lines[0], out int width) || !int.TryParse(lines[1], out int height))
            {
                throw new ExceptionParseFigure("Size format is incorrect.");
            }

            var figureData = new FigureData(new Vector2Int(width, height));
            var trueIndices = lines[2].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string trueIndex in trueIndices)
            {
                if (int.TryParse(trueIndex, out int linearIndex))
                {
                    figureData.SetTrue(linearIndex);
                }
                else
                {
                    throw new ExceptionParseFigure($"Invalid index value: {trueIndex}.");
                }
            }

            return figureData.Matrix;
        }
    }


}