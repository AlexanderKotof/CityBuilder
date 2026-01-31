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

        private bool SetBuilding(BuildingModel building, CellModel startLocation,
            IReadOnlyCollection<CellModel> occupiedCells)
        {
            foreach (var cell in occupiedCells)
            {
                if (cell.Content.Value != null)
                {
                    Debug.LogError($"Cell at position {startLocation.ToString()} is n't empty");
                    return false;
                }
                
                if (!BuildingsMap.TryAdd(cell, building))
                {
                    Debug.LogError($"Building at position {startLocation.ToString()} already exists! CHECK THIS!!");
                    return false;
                }
                
                cell.SetContent(building);
            }

            building.WorldPosition.Value = (startLocation.WorldPosition);
            building.SetOccupiedCells(occupiedCells);

            return true;
        }

        public void RemoveBuilding(BuildingModel building)
        {
            if (Buildings.Remove(building))
            {
                ClearBuildingCells(building);
                building.Dispose();
                Debug.Log($"Building removed {building.BuildingName}");
            }
        }

        private void ClearBuildingCells(BuildingModel building)
        {
            foreach (var cell in building.OccupiedCells)
            {
                cell.SetContent(null);
            }

            BuildingsMap.RemoveMany(building.OccupiedCells);
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

        public bool TryGetBuilding(CellModel location, out BuildingModel building) =>
            BuildingsMap.TryGetValue(location, out building);
    }
}