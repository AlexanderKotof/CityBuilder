using System;
using CityBuilder.Grid;
using GameSystems.Implementation.BuildingSystem.Domain;
using UnityEngine;

namespace GameSystems.Implementation.BuildingSystem.Extensions
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