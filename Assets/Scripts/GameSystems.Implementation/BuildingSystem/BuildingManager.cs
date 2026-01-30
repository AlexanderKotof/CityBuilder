using System;
using System.Linq;
using CityBuilder.Grid;
using Configs.Scriptable.Buildings;
using GameSystems.Implementation.BuildingSystem.Domain;
using UnityEngine;
using VContainer.Unity;

namespace GameSystems.Implementation.BuildingSystem
{
    public class BuildingManager : IInitializable, IDisposable
    {
        private readonly BuildingFactory _buildingFactory;
        private readonly BuildingsModel _model;
        private readonly BuildingsSettingsSo _config;
        private readonly GridManager _gridManager;

        public BuildingManager(BuildingsSettingsSo settingsSo, GridManager gridManager, BuildingFactory builderFactory, BuildingsModel model)
        {
            _gridManager = gridManager;
            _config = settingsSo;
            _model = model;
            _buildingFactory = builderFactory;
        }

        public void Initialize()
        {
            CreateStartBuilding();
        }       
        
        public void Dispose()
        {
            
        }

        private void CreateStartBuilding()
        {
            var grid = _gridManager.GridModels.First();

            var gridSize = grid.Size;

            var mainBuildingPosition = gridSize / 2 - Vector2Int.one;

            if (grid.TryGetCell(mainBuildingPosition, out var cellModel) == false)
            {
                Debug.LogError($"Can't find cell position for main building {mainBuildingPosition}!!!");
                return;
            }

            var config = _config.MainBuildingConfig;
            var building = _buildingFactory.Create(config, cellModel);
            SetBuilding(cellModel, building);
            
            _model.SetMainBuilding(building);
        }
        
        public void TryPlaceBuilding(CellModel cellModel, int configIndex)
        {
            if (_config.BuildingConfigs.Length <= configIndex || configIndex < 0)
            {
                return;
            }
            
            BuildingConfigSo config = _config.BuildingConfigs[configIndex];
            var building = _buildingFactory.Create(config, cellModel);
            
            if (CanPlaceBuilding(cellModel, building))
            {
                SetBuilding(cellModel, building);
            } 
        }

        public bool TryMoveBuilding(CellModel to, BuildingModel fromBuilding)
        {
            if (CanPlaceBuilding(to, fromBuilding))
            {
                MoveBuilding(fromBuilding, to);
                return true;
            }

            return false;
        }

        private void MoveBuilding(BuildingModel building, CellModel to) => _model.MoveBuilding(building, to);
        
        public bool TryGetBuilding(CellModel location, out BuildingModel building)
        {
            return _model.BuildingsMap.TryGetValue(location, out building);
        }

        private void SetBuilding(CellModel cellModel, BuildingModel building)
        {
            _model.AddBuilding(building, cellModel);
        }

        private bool CanPlaceBuilding(CellModel location, BuildingModel newBuilding)
        {
            var cells = newBuilding.GetBuildingCellsSet(location);
            foreach (var cell in cells)
            {
                if (_model.BuildingsMap.TryGetValue(cell, out var building) && building != newBuilding)
                {
                    return false;
                }
            }

            return true;
        }
    }
}