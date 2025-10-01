using System;
using System.Collections.Generic;
using CityBuilder.Grid;
using CityBuilder.Reactive;
using UnityEngine;

namespace CityBuilder.BuildingSystem
{
    public class BuildingsModel
    {
        public readonly Dictionary<CellModel, BuildingModel> BuildingsMap = new();
        
        public readonly ReactiveCollection<BuildingModel> Buildings = new();
        
        public BuildingModel MainBuilding { get; private set; }

        public void AddBuilding(BuildingModel building, CellModel location)
        {
            if (!BuildingsMap.TryAdd(location, building))
            {
                Debug.LogError($"Building at position {location.ToString()} already exists!");
                return;
            }
            
            var occupiedCells = GetBuildingCellsSet(building, location);

            foreach (var cell in occupiedCells)
            {
                //Is it really needed?
                cell.SetContent(building);
            }
            
            building.WorldPosition.Set(location.WorldPosition);
            building.SetOccupiedCells(occupiedCells);
            Buildings.Add(building);
            
            Debug.Log($"Building added, position {location.ToString()}");
        }
    
        public bool TryGetBuilding(CellModel location, out BuildingModel building) =>
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

        public void SetMainBuilding(BuildingModel building)
        {
            MainBuilding = building;
        }
        
        private IReadOnlyCollection<CellModel> GetBuildingCellsSet(BuildingModel building, CellModel startCell)
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