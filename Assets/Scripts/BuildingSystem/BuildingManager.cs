using System.Collections.Generic;
using UnityEngine;
using ViewSystem;

namespace BuildingSystem
{
    public class BuildingsModel
    {
        public Dictionary<CellModel, Building> BuildingsMap = new Dictionary<CellModel, Building>();

        public void AddBuilding(Building building, CellModel location)
        {
            foreach (var cell in GetBuildingCellsSet(building, location))
            {
                if (!BuildingsMap.TryAdd(location, building))
                {
                    Debug.LogError($"Building at position {location.ToString()} already exists!");
                }
                
                cell.SetContent(building);
            }
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

    public class BuildingManager
    {
        public BuildingFactory BuildingFactory { get; }
        public BuildingsConfig Config { get; }
        public BuildingsModel Model { get; } = new();
        
        private readonly GridManager _gridManager;
        private readonly ViewsProvider _viewsProvider;

        private readonly Transform _buildingsRoot;

        public BuildingManager(BuildingsConfig config, GridManager gridManager, ViewsProvider viewsProvider)
        {
            Config = config;
            _gridManager = gridManager;
            _viewsProvider = viewsProvider;

            BuildingFactory = new(viewsProvider);
            
            _buildingsRoot = new GameObject("---Buildings Root---").transform;
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
        
        public bool TryGetBuilding(CellModel location, out Building building)
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

        private void SetBuilding(CellModel cellModel, Building building)
        {
            Debug.Log("Placing building: " + building.Config.Name + " at: " + cellModel);
            
            Model.AddBuilding(building, cellModel);
            cellModel.SetContent(building);

            CreateBuildingView(building, cellModel);
        }

        private void CreateBuildingView(Building building, CellModel cell)
        {
            var worldPosition = cell.WorldPosition;
      
            var gameObject = _viewsProvider.ProvideView(building.Config.Prefab, _buildingsRoot);
            gameObject.transform.SetPositionAndRotation(worldPosition, Quaternion.identity);
            
            building.SetView(gameObject);
        }
        
        private void RemoveBuilding(CellModel cell)
        {
            if (Model.TryGetBuilding(cell, out var building))
            {
                Model.RemoveBuilding(cell);
                
                _viewsProvider.ReturnView(building.View);
                building.SetView(null);
                
                Debug.Log($"Building {building} removed from {cell}");
            }
        }

        public bool CanPlaceBuilding(CellModel location, Building newBuilding)
        {
            return !Model.BuildingsMap.TryGetValue(location, out var building) ||
                   CanBeUpgraded(building, newBuilding);
        }

        private bool CanBeUpgraded(Building first, Building second)
        {
            return first.Config == second.Config && first.Level == second.Level;
        }
        
        public bool CanMoveBuilding(CellModel location)
        {
            return Model.BuildingsMap.TryGetValue(location, out var building) && building.CanBeMoved;
        }
        
        
    }
}