using System.Linq;
using CityBuilder.Grid;
using UnityEditor;
using UnityEngine;
using ViewSystem;
using ViewSystem.Implementation;

namespace CityBuilder.BuildingSystem
{
    public class BuildingManager
    {
        public BuildingFactory BuildingFactory { get; }
        public BuildingsConfigSo Config { get; }
        public BuildingsModel Model { get; } = new();
        
        private readonly GridManager _gridManager;

        public BuildingManager(BuildingsConfigSo config, GridManager gridManager)
        {
            Config = config;
            _gridManager = gridManager;

            BuildingFactory = new();
            
            CreateStartBuilding();
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

            var config = Config.MainBuildingConfig;
            var building = BuildingFactory.Create(config, cellModel);
            SetBuilding(cellModel, building);
            
            Model.SetMainBuilding(building);
        }
        
        public void TryPlaceBuilding(CellModel cellModel, int configIndex)
        {
            if (Config.Configs.Length <= configIndex || configIndex < 0)
            {
                return;
            }
            
            var config = Config.Configs[configIndex];
            var building = BuildingFactory.Create(config, cellModel);
            
            if (CanPlaceBuilding(config, cellModel))
            {
                SetBuilding(cellModel, building);
            } 
        }

        public bool TryDragCellFromTo(CellModel from, CellModel to)
        {
            if (!TryGetBuilding(from, out var fromBuilding))
            {
                return false;
            }

            if (CanPlaceBuilding(fromBuilding.Config, to))
            {
                RemoveBuilding(from);
                SetBuilding(to, fromBuilding);
                
                return true;
            }
            
            if (TryGetBuilding(to, out var toBuilding) && CanBeUpgraded(toBuilding, fromBuilding))
            {
                RemoveBuilding(from);
                toBuilding.IncreaseLevel();
                
                Debug.Log($"Building level upgraded to {toBuilding.Level}");

                return true;
            }

            return false;
        }
        
        public bool TryGetBuilding(CellModel location, out BuildingModel building)
        {
            return Model.BuildingsMap.TryGetValue(location, out building);
        }

        public bool CanPlaceBuilding(BuildingConfig config, CellModel startCell)
        {
            var gridModel = startCell.GridModel;
            var position = startCell.Position;

            for (int i = position.X; i < position.X + config.Size.x; i++)
            {
                for (int j = position.Y; j < position.Y + config.Size.y; j++)
                {
                    var targetCell = gridModel.GetCell(i, j);
                    var targetContent = targetCell.Content.Value;
                     
                    if (targetCell.Content.HasValue() && targetContent.IsEmpty == false)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void SetBuilding(CellModel cellModel, BuildingModel building)
        {
            Model.AddBuilding(building, cellModel);
        }
        
        private void RemoveBuilding(CellModel cell)
        {
            if (Model.TryGetBuilding(cell, out var building))
            {
                Model.RemoveBuilding(cell);
            }
        }

        public bool CanPlaceBuilding(CellModel location, BuildingModel newBuilding)
        {
            return !Model.BuildingsMap.TryGetValue(location, out var building) ||
                   CanBeUpgraded(building, newBuilding);
        }

        private bool CanBeUpgraded(BuildingModel first, BuildingModel second)
        {
            return
                Equals(first.Config, second.Config) &&
                    first.Level.Value == second.Level.Value &&
                        Model.MainBuilding.Level.Value > first.Level.Value;
        }
        
        public bool CanMoveBuilding(CellModel location)
        {
            return Model.BuildingsMap.TryGetValue(location, out var building) && building.CanBeMoved;
        }
    }
}