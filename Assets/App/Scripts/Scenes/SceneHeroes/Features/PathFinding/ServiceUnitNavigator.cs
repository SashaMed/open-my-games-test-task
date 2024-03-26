using System;
using System.Collections.Generic;
using App.Scripts.Modules.Grid;
using App.Scripts.Scenes.SceneHeroes.Features.Grid.LevelInfo.Config;
using Assets.App.Scripts.Scenes.SceneHeroes.Features.PathFinding;
using UnityEngine;

namespace App.Scripts.Scenes.SceneHeroes.Features.PathFinding
{
    public class ServiceUnitNavigator : IServiceUnitNavigator
    {
        private Grid<int> grid;
        private UnitType currentUnit;


        public List<Vector2Int> FindPath(UnitType unitType, Vector2Int from, Vector2Int to, Grid<int> gridMatrix)
        {
            grid= gridMatrix;
            currentUnit= unitType;
            return FindPath(from,to);
        }

        private List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
        {
            var openSet = new PriorityQueue<Vector2Int, int>();
            openSet.Enqueue(start, 0);

            var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
            var gScore = new Dictionary<Vector2Int, int> { [start] = 0 };
            var fScore = new Dictionary<Vector2Int, int> { [start] = Heuristic(start, goal) };

            while (openSet.Count > 0)
            {
                var current = openSet.Dequeue();

                if (current.Equals(goal))
                {
                    return ReconstructPath(cameFrom, current);
                }

                foreach (var neighbor in GetNeighbors(current))
                {
                    var tentativeGScore = gScore[current] + 1; 
                    if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore;
                        fScore[neighbor] = tentativeGScore + Heuristic(neighbor, goal);

                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Enqueue(neighbor, fScore[neighbor]);
                        }
                    }
                }
            }

            return null; 
        }

        private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
        {
            var totalPath = new List<Vector2Int> { current };
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                totalPath.Insert(0, current);
            }
            return totalPath;
        }

        private IEnumerable<Vector2Int> GetNeighbors(Vector2Int node)
        {

            var directions = GetDirections();

            foreach (var direction in directions)
            {
                var nextNode = new Vector2Int(node.x + direction.x, node.y + direction.y);
                if (IsWalkable(nextNode))
                {
                    yield return nextNode;
                }
            }
        }


        private List<Vector2Int> GetDirections()
        {
            switch (currentUnit)
            {
                case UnitType.SwordMan:
                    return GetDirectionsForSwordMan();
                case UnitType.Angel:
                    return GetDirectionsForSwordMan();
                case UnitType.HorseMan:
                    return GetDirectionsForHorseMan();
                case UnitType.Barbarian:
                    return GetDirectionsForBarbarian();
                case UnitType.Poor:
                    return GetDirectionsForPoor();
                case UnitType.Shaman:
                    return GetDirectionsForShaman();
            }
            return null;
        }

        private List<Vector2Int> GetDirectionsForShaman()
        {
            return new List<Vector2Int>
            {
                new Vector2Int(2, 1),  new Vector2Int(2, -1),
                new Vector2Int(-2, 1), new Vector2Int(-2, -1),
                new Vector2Int(1, 2),  new Vector2Int(1, -2),
                new Vector2Int(-1, 2), new Vector2Int(-1, -2),
            };
        }

        private List<Vector2Int> GetDirectionsForPoor()
        {
            var result = new List<Vector2Int>
            {
                new Vector2Int(0, 1),
                new Vector2Int(0, -1)
            };
            result.AddRange(GetDirectionsForHorseMan());
            return result;
        }

        private List<Vector2Int> GetDirectionsForSwordMan()
        {
            return new List<Vector2Int>
            {
                new Vector2Int(1, 0),
                new Vector2Int(-1, 0),
                new Vector2Int(0, 1),
                new Vector2Int(0, -1)
            };
        }

        private List<Vector2Int> GetDirectionsForHorseMan()
        {
            return new List<Vector2Int>
            {
                new Vector2Int(1, 1),   
                new Vector2Int(1, -1),  
                new Vector2Int(-1, 1),  
                new Vector2Int(-1, -1), 
            };
        }

        private List<Vector2Int> GetDirectionsForBarbarian()
        {
            var result = GetDirectionsForSwordMan();
            result.AddRange(GetDirectionsForHorseMan());
            return result;
        }

        private bool IsWalkable(Vector2Int node)
        {
            if (node.x < 0 || node.x >= grid.Width || node.y < 0 || node.y >= grid.Height)
            {
                return false; 
            }
            var cellType = grid[node.x, node.y];
            return IsWalkableForCurrentUnit(cellType);
        }

        private bool IsWalkableForCurrentUnit(int cellType)
        {
            switch (currentUnit)
            {
                case UnitType.SwordMan:
                case UnitType.HorseMan:
                case UnitType.Poor:
                case UnitType.Shaman:
                    return cellType == ObstacleType.None;
                case UnitType.Angel:
                    return cellType != ObstacleType.Water;
                case UnitType.Barbarian:
                    return cellType != ObstacleType.Stone;
                default:
                    return false;
            }
        }

        private int Heuristic(Vector2Int a, Vector2Int b)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
        }
    }
}