using System;
using System.Collections.Generic;
using CityBuilder.Grid;
using CityBuilder.Reactive;
using UnityEngine;

namespace CityBuilder.BuildingSystem
{
    public class BuildingsModel
    {
        public readonly Dictionary<CellModel, Building> BuildingsMap = new Dictionary<CellModel, Building>();
        
        public readonly ReactiveCollection<Building> Buildings = new ReactiveCollection<Building>();

        public void AddBuilding(Building building, CellModel location)
        {
            var occupiedCells = GetBuildingCellsSet(building, location);
            foreach (var cell in occupiedCells)
            {
                if (!BuildingsMap.TryAdd(location, building))
                {
                    Debug.LogError($"Building at position {location.ToString()} already exists!");
                }
                
                //Is it really needed?
                cell.SetContent(building);
            }
            
            building.WorldPosition.Set(location.WorldPosition);
            building.SetOccupiedCells(occupiedCells);
            Buildings.Add(building);
            
            Debug.Log($"Building added, position {location.ToString()}");
        }
    
        public bool TryGetBuilding(CellModel location, out Building building) =>
            BuildingsMap.TryGetValue(location, out building);
        
        public void RemoveBuilding(CellModel location)
        {
            var building = BuildingsMap[location];
            foreach (var cell in GetBuildingCellsSet(building, location))
            {
                if (!BuildingsMap.Remove(cell))
                {
                    Debug.LogError($"No Building found at position {cell.ToString()}!");
                }
                
                cell.SetContent(null);
            }
            
            building.SetOccupiedCells(Array.Empty<CellModel>());
            Buildings.Remove(building);
            
            Debug.Log($"Building removed from position {location.ToString()}");
        }
        
        private IReadOnlyCollection<CellModel> GetBuildingCellsSet(Building building, CellModel startCell)
        {
            var list = new List<CellModel>();
            var position = startCell.Position;
            var config = building.Config;
            
            for (int i = position.X; i < position.X + config.Size.x; i++)
            {
                for (int j = position.Y; j < position.Y + config.Size.y; j++)
                {
                    list.Add(startCell.GridModel.GetCell(i, j));
                }
            }

            return list;
        }
    }
}