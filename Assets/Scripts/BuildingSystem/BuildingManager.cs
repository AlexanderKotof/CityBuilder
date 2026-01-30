using System;
using System.Linq;
using CityBuilder.Grid;
using Configs.Scriptable.Buildings;
using UnityEngine;
using VContainer.Unity;

namespace BuildingSystem
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
            
            if (CanPlaceBuilding(config, cellModel))
            {
                SetBuilding(cellModel, building);
            } 
        }
        
        //TODO: this is responsibility of merge system/service
        //THIS WORKS NOT REALLY LIKE IT SHOULD
        public bool TryDragCellFromTo(CellModel from, CellModel to)
        {
            if (!TryGetBuilding(from, out var fromBuilding))
            {
                return false;
            }

            if (TryMoveBuilding(to, fromBuilding)) 
                return true;
            
            return false;
        }

        public bool TryMoveBuilding(CellModel to, BuildingModel fromBuilding)
        {
            if (CanPlaceBuilding(fromBuilding.Config, to))
            {
                MoveBuilding(fromBuilding, to);
                return true;
            }

            return false;
        }

        public void MoveBuilding(BuildingModel building, CellModel to)
        {
            _model.MoveBuilding(building, to);
        }
        
        public bool TryGetBuilding(CellModel location, out BuildingModel building)
        {
            return _model.BuildingsMap.TryGetValue(location, out building);
        }

        public bool CanPlaceBuilding(BuildingConfigSo config, CellModel startCell)
        {
            var gridModel = startCell.GridModel;
            var position = startCell.Position;

            for (int i = position.X; i < position.X + config.Size.X; i++)
            {
                for (int j = position.Y; j < position.Y + config.Size.Y; j++)
                {
                    var targetCell = gridModel.GetCell(i, j);
                    var targetContent = targetCell.Content.Value;
                     
                    if (targetCell.Content.Value != null && targetContent.IsEmpty == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void SetBuilding(CellModel cellModel, BuildingModel building)
        {
            _model.AddBuilding(building, cellModel);
        }

        public void RemoveBuilding(CellModel cell)
        {
            if (_model.TryGetBuilding(cell, out var building))
            {
                _model.RemoveBuildingAt(cell);
            }
        }

        public bool CanPlaceBuilding(CellModel location, BuildingModel newBuilding)
        {
            var cells = newBuilding.GetBuildingCellsSet(location);
            foreach (var cell in cells)
            {
                if (_model.BuildingsMap.TryGetValue(cell, out var building) && building != newBuilding)
                {
                    return false;
                }
            }

            return true; //CanBeUpgraded(building, newBuilding);
        }

        private bool CanBeUpgraded(BuildingModel first, BuildingModel second)
        {
            return
                Equals(first.Config, second.Config) &&
                    first.Level.Value == second.Level.Value &&
                        _model.MainBuilding.Level.Value > first.Level.Value;
        }
    }
}