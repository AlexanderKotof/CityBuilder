using System;
using CityBuilder.Configs.Scriptable;
using CityBuilder.GameSystems.Common.ViewSystem.ViewsProvider;
using CityBuilder.GameSystems.Implementation.BuildingSystem;
using CityBuilder.GameSystems.Implementation.BuildingSystem.Features;
using CityBuilder.GameSystems.Implementation.CellGridFeature.Grid;
using CityBuilder.GameSystems.Implementation.GameInteractionFeature.InteractionStateMachine;
using UnityEngine;
using VContainer.Unity;

namespace CityBuilder.GameSystems.Implementation.GameInteractionFeature
{
    public class GameInteractionFeature : IInitializable, IDisposable
    {
        private readonly BuildingManager _buildingManager;
        private readonly MergeBuildingsFeature _mergeBuildingsFeature;
        private PlayerInteractionStateMachine? _playerInteractionStateMachine;
    
        private readonly IViewsProvider _viewsProvider;
        private readonly CommonGameSettingsSo _settings;
        private Transform _cursor;
        private CellSelectionController _cellSelectionController;

        public GameInteractionFeature(BuildingManager buildingManager, MergeBuildingsFeature mergeBuildingsFeature)
        {
            _buildingManager = buildingManager;
            _mergeBuildingsFeature = mergeBuildingsFeature;
        }

        public void Dispose()
        {
        }

        public void Initialize()
        {
            
        }

        public bool TryDropContent(CellModel fromCell, CellModel toCellModel)
        {
            if (!_buildingManager.TryGetBuilding(fromCell, out var fromBuilding))
                return false;
            
            if (_buildingManager.TryMoveBuilding(toCellModel, fromBuilding)) 
                return true;
            
            if (!_buildingManager.TryGetBuilding(toCellModel, out var toBuilding))
                return false;

            return _mergeBuildingsFeature.TryMergeBuildingsFromTo(fromBuilding, toBuilding);
        }
    }
}