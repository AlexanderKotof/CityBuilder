using System;
using System.Collections.Generic;
using System.Threading;
using BuildingSystem;
using Configs.Scriptable;
using Configs.Scriptable.Battle;
using Cysharp.Threading.Tasks;
using GameSystems.Common.ViewSystem;
using GameSystems.Common.ViewSystem.ViewsProvider;
using UniRx;
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
        private readonly ViewsCollectionController<BattleUnitUIComponent> _buildingsUi;
        private readonly IViewsProvider _viewsProvider;
        private BattleUnitUIComponent _uiView;
        private readonly CompositeDisposable _disposables = new();

        public PlayerBuildingsUnitsController(BattleSystemModel battleSystemModel, BattleUnitsConfigSO config, BuildingsModel buildingsModel, IViewsProvider viewsProvider)
        {
            _battleSystemModel = battleSystemModel;
            _config = config;
            _buildingsModel = buildingsModel;
            _buildingsUi = new ViewsCollectionController<BattleUnitUIComponent>(viewsProvider, defaultAssetKey: config.BattleUiAssetKey);
        }

        public void Init()
        {
            _buildingsModel.Buildings.SubscribeAdd(OnBuildingAdded);
            _buildingsModel.Buildings.SubscribeRemove(OnBuildingRemoved);

            foreach (var building in _buildingsModel.Buildings)
            {
                OnBuildingAdded(building);
            }

            SubscribePlayerBuildingsUnits();
        }

        private void SubscribePlayerBuildingsUnits()
        {
            _battleSystemModel.PlayerBuildingsUnits
                .SubscribeToCollection(data => AddPlayerBuildingUnit(data).Forget(), RemovePlayerBuildingUnit)
                .AddTo(_disposables);
            async UniTaskVoid AddPlayerBuildingUnit(BattleUnitBase unit)
            {
                var view = await _buildingsUi.AddView(unit, unit.ThisTransform.Value);
                view.Init(unit);
                
                // view.transform.SetParent();
                // view.transform.localPosition = Vector3.zero;

                // async UniTaskVoid OnTransformUpdated(Transform value)
                // {
                //     if (value == null)
                //         return;
                //
                //     if (battleUnit.ThisTransform.Value == value)
                //     {
                //         return;
                //     }
                //
                //     building.ThisTransform.Unsubscribe(handle);
                //
                //     battleUnit.ThisTransform.Set(value);
                //
                //     // if (_uiView != null)
                //     // {
                //     //     _viewsProvider.ReturnView(_uiView);
                //     // }
                //
                //     _uiView = await _viewsProvider.ProvideViewAsync<BattleUnitUIComponent>(_config.BattleUiAssetKey, value);
                //     _uiView.Init(battleUnit);
                // }
            }

            void RemovePlayerBuildingUnit(BattleUnitBase unit)
            {
                _buildingsUi.Recycle(unit);
            }
        }

        public void Deinit()
        {
            _disposables.Dispose();
            
            _buildingsModel.Buildings.UnsubscribeAdd(OnBuildingAdded);
            _buildingsModel.Buildings.UnsubscribeRemove(OnBuildingRemoved);
            
            // if (_uiView != null)
            // {
            //     _viewsProvider.ReturnView(_uiView);
            // }
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
            var config = building.Config._unitConfig != null ? building.Config._unitConfig : _config.DefaultBuildingUnit;
            var battleUnit = new BattleUnitBase(config, building.Level, building.WorldPosition, building.ThisTransform.Value);
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