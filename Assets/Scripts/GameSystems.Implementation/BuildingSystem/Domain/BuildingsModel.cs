using System;
using System.Collections.Generic;
using CityBuilder.Grid;
using GameSystems.Implementation.BattleSystem;
using UniRx;
using UnityEngine;

namespace GameSystems.Implementation.BuildingSystem.Domain
{
    public class BuildingsModel
    {
        public readonly Dictionary<CellModel, BuildingModel> BuildingsMap = new();
        
        public readonly ReactiveCollection<BuildingModel> Buildings = new();
        
        public BuildingModel MainBuilding { get; private set; }

        public void AddBuilding(BuildingModel building, CellModel startLocation)
        {
            var occupiedCells = building.GetBuildingCellsSet(startLocation);
            if (SetBuilding(building, startLocation, occupiedCells) == false) 
                throw new Exception("Could not set building for this cell set!");
            
            Buildings.Add(building);
            
            Debug.Log($"Building {building.BuildingName} added, position {startLocation.ToString()}");
        }

        private bool SetBuilding(BuildingModel building, CellModel startLocation, IReadOnlyCollection<CellModel> occupiedCells)
        {
            foreach (var cell in occupiedCells)
            {
                if (!BuildingsMap.TryAdd(cell, building))
                {
                    Debug.LogError($"Building at position {startLocation.ToString()} already exists! CHECK THIS!!");
                    return false;
                }
                
                //Is it really needed?
                cell.SetContent(building);
            }
            
            building.WorldPosition.Value = (startLocation.WorldPosition);
            building.SetOccupiedCells(occupiedCells);

            return true;
        }

        public bool TryGetBuilding(CellModel location, out BuildingModel building) =>
            BuildingsMap.TryGetValue(location, out building);
        
        public void RemoveBuildingAt(CellModel location)
        {
            RemoveBuilding(BuildingsMap[location]);
        }

        public void RemoveBuilding(BuildingModel building)
        {
            ClearBuildingCells(building);
            
            Buildings.Remove(building);
            
            Debug.Log($"Building removed {building.BuildingName} from {building.WorldPosition.ToString()}");
        }

        private void ClearBuildingCells(BuildingModel building)
        {
            foreach (var cell in building.OccupiedCells)
            {
                if (!BuildingsMap.Remove(cell))
                {
                    Debug.LogError($"No Building found at position {cell.ToString()}! CHECK THIS!");
                }
                
                cell.SetContent(null);
            }

            BuildingsMap.RemoveMany(building.OccupiedCells);
            building.SetOccupiedCells(Array.Empty<CellModel>());
        }

        public void SetMainBuilding(BuildingModel building)
        {
            MainBuilding = building;
        }
        

        
        public void MoveBuilding(BuildingModel building, CellModel to)
        {
            ClearBuildingCells(building);

            var cells = building.GetBuildingCellsSet(to);
            SetBuilding(building, to, cells);
            building.SetOccupiedCells(cells);
        }
    }

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