using System.Collections.Generic;
using System.Linq;
using CityBuilder.Grid;
using UnityEngine;

namespace GameSystems.Implementation.BuildingSystem.Domain
{
    public static class BuildingModelExtensions
    {
        /// <summary>
        /// TODO: не учитывает поворот
        /// </summary>
        /// <param name="building"></param>
        /// <param name="startCell"></param>
        /// <returns></returns>
        public static IReadOnlyCollection<CellModel> GetBuildingCellsSet(this BuildingModel building, CellModel startCell)
        {
            var list = new List<CellModel>();
            
            var gridModel = startCell.GridModel;
            var position = startCell.Position;
            var config = building.Config;
            
            for (int i = position.X; i < position.X + config.Size.X; i++)
            {
                for (int j = position.Y; j < position.Y + config.Size.Y; j++)
                {
                    list.Add(gridModel.GetCell(i, j));
                }
            }

            return list;
        }
        
        public static IEnumerable<CellModel> GetAllNearCellsExceptOwn(this BuildingModel building)
        {
            var list = new HashSet<CellModel>();
            var buildingCells = building.OccupiedCells;
            var first = buildingCells.First();
            var gridModel = first.GridModel;
            var position = first.Position;

            int fromX;
            int fromY;
            int toX;
            int toY;
            if (buildingCells.Count == 1)
            {
                fromX = Mathf.Clamp(position.X - 1, 0, gridModel.Size.x);
                fromY = Mathf.Clamp(position.Y - 1, 0, gridModel.Size.y);
                toX = Mathf.Clamp(position.X + 1, 0, gridModel.Size.x);
                toY = Mathf.Clamp(position.Y + 1, 0, gridModel.Size.y);
                
                for (int i = fromX; i <= toX; i++)
                {
                    for (int j = fromY; j <= toY; j++)
                    {
                        if (position.X == i && position.Y == j)
                            continue;
                        
                        list.Add(gridModel.GetCell(i, j));
                    }
                }
                return list;
            }
            
            var xPositions = buildingCells.Select(c => c.Position.X).ToArray();
            var yPositions = buildingCells.Select(c => c.Position.Y).ToArray();

            fromX = Mathf.Clamp(Mathf.Min(xPositions), 0, gridModel.Size.x); 
            fromY = Mathf.Clamp(Mathf.Min(yPositions), 0, gridModel.Size.y); 
            toX = Mathf.Clamp(Mathf.Max(xPositions), 0, gridModel.Size.x); 
            toY = Mathf.Clamp(Mathf.Max(yPositions), 0, gridModel.Size.y); 
                
            for (int i = fromX; i <= toX; i++)
            {
                for (int j = fromY; j <= toY; j++)
                {
                    if (buildingCells.Any(cell => cell.Position.Value == new Vector2Int(i, j)))
                        continue;
                        
                    list.Add(gridModel.GetCell(i, j));
                }
            }
            
            return list;
        }
    }
}