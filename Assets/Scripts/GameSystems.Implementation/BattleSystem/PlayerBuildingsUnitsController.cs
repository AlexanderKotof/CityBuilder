using System;
using System.Collections.Generic;
using CityBuilder.BuildingSystem;
using Configs.Schemes.BattleSystem;
using UnityEngine;

namespace GameSystems.Implementation.BattleSystem
{
    public class PlayerBuildingsUnitsController
    {
        private readonly BattleUnitsModel _battleUnitsModel;
        private readonly BattleUnitsConfigScheme _config;
        private readonly BuildingsModel _buildingsModel;

        private readonly Dictionary<Guid, BattleUnitBase> _battleUnitsByBuildingRuntimeId = new();

        public PlayerBuildingsUnitsController(BattleUnitsModel battleUnitsModel, BattleUnitsConfigScheme config, BuildingsModel buildingsModel)
        {
            _battleUnitsModel = battleUnitsModel;
            _config = config;
            _buildingsModel = buildingsModel;
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
            _battleUnitsModel.AddPlayerBuilding(buildingUnit);
            return;
            
            void OnBuildingUnitDestroyed(IBattleUnit _)
            {
                Debug.Log($"Building {building.BuildingName} destroyed!");

                buildingUnit.OnUnitDied -= OnBuildingUnitDestroyed;
                _buildingsModel.RemoveBuilding(building);
            }
        }
        
        private BattleUnitBase CreateBattleUnit(BuildingModel building)
        {
            var config = building.Config.UnitConfig?.Value ?? _config.DefaultBuildingUnit;
            var unitConfig = new BattleUnitBase(config);

            if (building.ThisTransform.Value != null)
            {
                unitConfig.ThisTransform.Set(building.ThisTransform.Value);
            }
            else
            {
                building.ThisTransform.AddListener(OnTransformUpdated);
            }

            return unitConfig;

            void OnTransformUpdated(Transform value)
            {
                building.ThisTransform.RemoveListener(OnTransformUpdated);
                unitConfig.ThisTransform.Set(value);
            }
        }
        
        private void OnBuildingRemoved(BuildingModel building)
        {
            if (_battleUnitsByBuildingRuntimeId.Remove(building.RuntimeId, out var buildingUnit))
            {
                buildingUnit.Dispose();
            }
        }
    }
}