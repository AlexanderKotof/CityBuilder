using System;
using System.Collections.Generic;
using CityBuilder.Grid;
using CityBuilder.Reactive;
using UnityEngine;

namespace BuildingSystem
{
    public class BuildingsModel
    {
        public readonly Dictionary<CellModel, BuildingModel> BuildingsMap = new();
        
        public readonly ReactiveCollection<BuildingModel> Buildings = new();
        
        public BuildingModel MainBuilding { get; private set; }

        public void AddBuilding(BuildingModel building, CellModel startLocation)
        {
            var occupiedCells = GetBuildingCellsSet(building, startLocation);

            foreach (var cell in occupiedCells)
            {
                if (!BuildingsMap.TryAdd(cell, building))
                {
                    Debug.LogError($"Building at position {startLocation.ToString()} already exists!");
                    return;
                }
                
                //Is it really needed?
                cell.SetContent(building);
            }
            
            building.WorldPosition.Value = (startLocation.WorldPosition);
            building.SetOccupiedCells(occupiedCells);
            Buildings.Add(building);
            
            Debug.Log($"Building added, position {startLocation.ToString()}");
        }
    
        public bool TryGetBuilding(CellModel location, out BuildingModel building) =>
            BuildingsMap.TryGetValue(location, out building);
        
        public void RemoveBuilding(CellModel location)
        {
            RemoveBuilding(BuildingsMap[location]);
        }

        public void RemoveBuilding(BuildingModel building)
        {
            foreach (var cell in building.OccupiedCells)
            {
                if (!BuildingsMap.Remove(cell))
                {
                    Debug.LogError($"No Building found at position {cell.ToString()}! CHECK THIS!");
                }
                
                cell.SetContent(null);
            }
            
            building.SetOccupiedCells(Array.Empty<CellModel>());
            Buildings.Remove(building);
            
            Debug.Log($"Building removed {building.BuildingName} from {building.WorldPosition.ToString()}");
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
            
            for (int i = position.X; i < position.X + config._size.X; i++)
            {
                for (int j = position.Y; j < position.Y + config._size.Y; j++)
                {
                    list.Add(startCell.GridModel.GetCell(i, j));
                }
            }

            return list;
        }
    }
}