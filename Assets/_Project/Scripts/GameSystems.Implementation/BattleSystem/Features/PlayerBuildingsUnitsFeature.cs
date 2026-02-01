using System;
using System.Collections.Generic;
using CityBuilder.Configs.Scriptable.Battle;
using CityBuilder.GameSystems.Common.ViewSystem;
using CityBuilder.GameSystems.Common.ViewSystem.ViewsProvider;
using CityBuilder.GameSystems.Implementation.BattleSystem.Domain;
using CityBuilder.GameSystems.Implementation.BattleSystem.Specific;
using CityBuilder.GameSystems.Implementation.BuildingSystem.Domain;
using CityBuilder.Utilities.Extensions;
using CityBuilder.Views.Implementation.BattleSystem;
using UniRx;
using VContainer.Unity;

namespace CityBuilder.GameSystems.Implementation.BattleSystem.Features
{
    /// <summary>
    /// Контроллирует сущьность боевого юнита, приатаченного к зданию (для всей коллекции зданий игрока)
    /// </summary>
    public class PlayerBuildingsUnitsFeature : IInitializable, IDisposable
    {
        private readonly BattleSystemModel _battleSystemModel;
        private readonly BattleUnitsConfigSO _config;
        private readonly BuildingsModel _buildingsModel;

        private readonly Dictionary<Guid, BuildingBattleUnitController> _controllersByBuildingRuntimeId = new();
        private readonly ViewsCollectionController<BattleUnitUIComponent> _buildingsUi;
        private readonly IViewsProvider _viewsProvider;
        private BattleUnitUIComponent _uiView;
        private readonly CompositeDisposable _disposables = new();
        private readonly BattleUnitsFactory _battleUnitsFactory;

        public PlayerBuildingsUnitsFeature(BattleSystemModel battleSystemModel, BattleUnitsConfigSO config, BuildingsModel buildingsModel, IViewsProvider viewsProvider, BattleUnitsFactory battleUnitsFactory)
        {
            _battleSystemModel = battleSystemModel;
            _config = config;
            _buildingsModel = buildingsModel;
            _battleUnitsFactory = battleUnitsFactory;
            _buildingsUi = new ViewsCollectionController<BattleUnitUIComponent>(viewsProvider, defaultAssetKey: config.BattleUiAssetKey);
        }

        public void Initialize()
        {
            _buildingsModel.Buildings
                .SubscribeToCollection(OnBuildingAdded, OnBuildingRemoved)
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            foreach (var controller in _controllersByBuildingRuntimeId.Values)
            {
                controller.Dispose();
            }
            _controllersByBuildingRuntimeId.Clear();
            
            _buildingsUi?.Dispose();
            _disposables?.Dispose();
        }

        private void OnBuildingAdded(BuildingModel building)
        {
            var controller = new BuildingBattleUnitController(_buildingsUi, building, _battleUnitsFactory, _battleSystemModel, _buildingsModel);
            controller.Initialize();
            _controllersByBuildingRuntimeId.Add(building.RuntimeId, controller);
        }
        
        // After building removed we remove the controller
        private void OnBuildingRemoved(BuildingModel building)
        {
            if (_controllersByBuildingRuntimeId.Remove(building.RuntimeId, out var controller))
            {
                controller.Dispose();
            }
        }
    }
}