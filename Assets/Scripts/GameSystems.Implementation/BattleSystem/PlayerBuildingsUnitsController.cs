using System;
using System.Collections.Generic;
using Configs.Scriptable.Battle;
using Cysharp.Threading.Tasks;
using GameSystems.Common.ViewSystem;
using GameSystems.Common.ViewSystem.ViewsProvider;
using GameSystems.Implementation.BuildingSystem;
using GameSystems.Implementation.BuildingSystem.Domain;
using UniRx;
using UnityEngine;

namespace GameSystems.Implementation.BattleSystem
{
    public class PlayerBuildingsUnitsController
    {
        private readonly BattleSystemModel _battleSystemModel;
        private readonly BattleUnitsConfigSO _config;
        private readonly BuildingsModel _buildingsModel;

        private readonly Dictionary<Guid, BattleUnitBase> _battleUnitsByBuildingRuntimeId = new();
        private readonly ViewsCollectionController<BattleUnitUIComponent> _buildingsUi;
        private readonly IViewsProvider _viewsProvider;
        private BattleUnitUIComponent _uiView;
        private readonly CompositeDisposable _disposables = new();
        private readonly Dictionary<BattleUnitBase, IDisposable> _disposablesByUnit = new();

        public PlayerBuildingsUnitsController(BattleSystemModel battleSystemModel, BattleUnitsConfigSO config, BuildingsModel buildingsModel, IViewsProvider viewsProvider)
        {
            _battleSystemModel = battleSystemModel;
            _config = config;
            _buildingsModel = buildingsModel;
            _buildingsUi = new ViewsCollectionController<BattleUnitUIComponent>(viewsProvider, defaultAssetKey: config.BattleUiAssetKey);
        }

        public void Init()
        {
            _buildingsModel.Buildings
                .SubscribeToCollection(OnBuildingAdded, OnBuildingRemoved)
                .AddTo(_disposables);

            SubscribePlayerBuildingsUnits();
        }

        private void SubscribePlayerBuildingsUnits()
        {
            _battleSystemModel.PlayerBuildingsUnits
                .SubscribeToCollection(data => AddPlayerBuildingUnit(data).Forget(), RemovePlayerBuildingUnit)
                .AddTo(_disposables);
            async UniTaskVoid AddPlayerBuildingUnit(BattleUnitBase unit)
            {
                var uiView = await _buildingsUi.AddView(unit, unit.ThisTransform.Value);
                uiView.Init(unit);
                
                var disposible = unit.ThisTransform.Subscribe(OnTransformUpdated);
                _disposablesByUnit.Add(unit, disposible);
                OnTransformUpdated(unit.ThisTransform.Value);
                return;
                
                void OnTransformUpdated(Transform value)
                {
                    if (value == null)
                        return;
                    
                    Debug.LogError($"Transform updated for {unit.Config.name}!", value);
                    
                    uiView.transform.SetParent(value);
                    uiView.transform.localPosition = Vector3.up;
                }
            }

            void RemovePlayerBuildingUnit(BattleUnitBase unit)
            {
                _buildingsUi.Return(unit);

                if (_disposablesByUnit.Remove(unit, out var disposable))
                {
                    disposable.Dispose();
                }
            }
        }

        public void Deinit()
        {
            _disposables.Dispose();
            
            foreach (var disposable in _disposablesByUnit.Values)
            {
                disposable.Dispose();
            }
            _disposablesByUnit.Clear();
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
        
        //TODO: to units factory
        private BattleUnitBase CreateBattleUnit(BuildingModel building)
        {
            var config = building.Config.UnitConfig != null ? building.Config.UnitConfig : _config.DefaultBuildingUnit;
            
            //TODO: inherit some properties?
            var battleUnit = new BattleUnitBase(config, building.Level.Value, building.WorldPosition.Value, building.ThisTransform);
            return battleUnit;
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