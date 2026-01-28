using System;
using System.Collections.Generic;
using CityBuilder.BuildingSystem;
using CityBuilder.Dependencies;
using Configs.Schemes.BattleSystem;
using Configs.Scriptable;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Views.Implementation.BattleSystem;
using ViewSystem;

namespace GameSystems.Implementation.BattleSystem
{
    public class PlayerBuildingsUnitsController
    {
        private readonly BattleSystemModel _battleSystemModel;
        private readonly BattleUnitsConfigSO _config;
        private readonly BuildingsModel _buildingsModel;

        private readonly Dictionary<Guid, BattleUnitBase> _battleUnitsByBuildingRuntimeId = new();
        private readonly BattleUnitsViewsCollection _buildingsUi;
        private readonly IViewsProvider _viewsProvider;

        public PlayerBuildingsUnitsController(BattleSystemModel battleSystemModel, BattleUnitsConfigSO config, BuildingsModel buildingsModel, IViewsProvider viewsProvider)
        {
            _battleSystemModel = battleSystemModel;
            _config = config;
            _buildingsModel = buildingsModel;
            _viewsProvider = viewsProvider;
        }

        public void Init()
        {
            _buildingsModel.Buildings.SubscribeAdd(OnBuildingAdded);
            _buildingsModel.Buildings.SubscribeRemove(OnBuildingRemoved);

            foreach (var building in _buildingsModel.Buildings)
            {
                OnBuildingAdded(building);
            }
        }

        public void Deinit()
        {
            _buildingsModel.Buildings.UnsubscribeAdd(OnBuildingAdded);
            _buildingsModel.Buildings.UnsubscribeRemove(OnBuildingRemoved);
        }

        private void OnBuildingAdded(BuildingModel building)
        {
            var buildingUnit = CreateBattleUnit(building);
            buildingUnit.OnUnitDied += OnBuildingUnitDestroyed;
            _battleUnitsByBuildingRuntimeId.Add(building.RuntimeId, buildingUnit);
            _battleSystemModel.AddPlayerBuilding(buildingUnit);

            if (_buildingsModel.MainBuilding == building)
            {
                Debug.LogError("Created main building battle model...");
                _battleSystemModel.SetMainBuilding(buildingUnit);
            }
            return;

            void OnBuildingUnitDestroyed(IBattleUnit _)
            {
                Debug.LogError($"Building {building.BuildingName} destroyed!");

                buildingUnit.OnUnitDied -= OnBuildingUnitDestroyed;
                
                
                if (_buildingsModel.MainBuilding == building)
                {
                    Debug.LogError("Destroyed main building battle model... GAME OVER");
                }
                
                _buildingsModel.RemoveBuilding(building);

           
            }
        }
        
        private BattleUnitBase CreateBattleUnit(BuildingModel building)
        {
            var config = building.Config.UnitConfig != null ? building.Config.UnitConfig : _config.DefaultBuildingUnit;
            var battleUnit = new BattleUnitBase(config, building.Level, building.WorldPosition);
            
            Action<Transform> handle = (value) => OnTransformUpdated(value).Forget();
            handle += _ => building.ThisTransform.Unsubscribe(handle);
            building.ThisTransform.Subscribe(handle, true);

            return battleUnit;

            async UniTaskVoid OnTransformUpdated(Transform value)
            {
                battleUnit.ThisTransform.Set(value);

                var uiView = await _viewsProvider.ProvideViewAsync<BattleUnitUIComponent>(_config.BattleUiAssetKey, value);
                uiView.Init(battleUnit);
            }
        }

        private void OnBuildingRemoved(BuildingModel building)
        {
            if (_battleUnitsByBuildingRuntimeId.Remove(building.RuntimeId, out var buildingUnit))
            {
                Debug.LogError("Removed building battle unit");
                
                _battleSystemModel.RemoveUnit(buildingUnit);
            }
        }
    }
}