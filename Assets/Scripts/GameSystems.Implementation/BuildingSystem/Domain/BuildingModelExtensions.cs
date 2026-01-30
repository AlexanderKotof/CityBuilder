using System.Collections.Generic;
using CityBuilder.Grid;

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
    }
}