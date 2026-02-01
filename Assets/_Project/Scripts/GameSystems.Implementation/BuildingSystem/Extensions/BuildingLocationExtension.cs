using System;
using CityBuilder.GameSystems.Implementation.BuildingSystem.Domain;
using CityBuilder.GameSystems.Implementation.CellGridFeature.Grid;
using UnityEngine;

namespace CityBuilder.GameSystems.Implementation.BuildingSystem.Extensions
{
    [Obsolete]
    public static class BuildingLocationExtension
    {
        public static Vector3 GetWorldPosition(this BuildingLocation location)
        {
            return location.Grid.GridPositionToCellWorldPosition(location.Position);
        }

        public static CellModel GetCell(this BuildingLocation location)
        {
            return location.Grid.GetCell(location.Position);
        }
    }
}