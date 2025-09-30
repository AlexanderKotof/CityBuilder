using CityBuilder.Dependencies;
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

        private readonly BuildingViewCollection _buildingViewsController;
        
        private readonly GridManager _gridManager;
        private readonly IViewsProvider _viewsProvider;
        private readonly ViewWithModelProvider _viewWithModelProvider;

        public BuildingManager(BuildingsConfigSo config, GridManager gridManager, IViewsProvider viewsProvider)
        {
            Config = config;
            _gridManager = gridManager;
            _viewsProvider = viewsProvider;

            BuildingFactory = new(viewsProvider);
            
            _viewWithModelProvider = new ViewWithModelProvider(viewsProvider, new DependencyContainer());

            _buildingViewsController = new (Model, _viewWithModelProvider);
            _buildingViewsController.Initialize();
        }

        public void Deinit()
        {
            _buildingViewsController.Deinit();
            _viewWithModelProvider.Deinit();
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

        public void TryPlaceDefaultBuilding(CellModel cellModel)
        {
            var config = Config.Configs[0];
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
            return string.Equals(first.Config.Name, second.Config.Name) &&
                   first.Level.Value == second.Level.Value;
        }
        
        public bool CanMoveBuilding(CellModel location)
        {
            return Model.BuildingsMap.TryGetValue(location, out var building) && building.CanBeMoved;
        }
    }
}