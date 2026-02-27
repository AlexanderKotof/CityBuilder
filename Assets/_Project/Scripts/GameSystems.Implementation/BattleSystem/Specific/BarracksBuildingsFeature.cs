using System;
using System.Collections.Generic;
using CityBuilder.Configs.Scriptable.Buildings.Functions;
using CityBuilder.GameSystems.Implementation.BattleSystem.Domain;
using CityBuilder.GameSystems.Implementation.BattleSystem.Domain.Units;
using CityBuilder.GameSystems.Implementation.BuildingSystem.Domain;
using CityBuilder.GameSystems.Implementation.BuildingSystem.Extensions;
using CityBuilder.Utilities.Extensions;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace CityBuilder.GameSystems.Implementation.BattleSystem.Specific
{
    public class BarracksBuildingsFeature : IInitializable, IDisposable, ITickable
    {
        private readonly BuildingsModel _buildingModel;
        private readonly BattleManager _battleManager;
        private readonly BattleSystemModel _battleSystemModel;

        private readonly CompositeDisposable _subscriptions = new();
        
        private readonly Dictionary<BuildingModel, BarrackBuildingProcessor> _barracksProcessors = new();

        public BarracksBuildingsFeature(BuildingsModel buildingModel, BattleManager battleManager, BattleSystemModel battleSystemModel)
        {
            _buildingModel = buildingModel;
            _battleManager = battleManager;
            _battleSystemModel = battleSystemModel;
        }
        
        public void Initialize()
        {
            _buildingModel.Buildings
                .SubscribeToCollection(OnBuildingAdded, OnBuildingRemoved, true)
                .AddTo(_subscriptions);
        }
        
        private void OnBuildingAdded(BuildingModel building)
        {
            if (building.Config.TryGetBuildingFunction<BarracksBuildingFunctionSo>(out var function) == false)
            {
                return;
            }

            var processor = new BarrackBuildingProcessor(building, function, _battleSystemModel, _battleManager);
            _barracksProcessors.Add(building, processor);
        }
        
        private void OnBuildingRemoved(BuildingModel building)
        {
            if (_barracksProcessors.Remove(building, out var processor) == false)
            {
                return;
            }

            processor.Stop();
        }

        public void Dispose()
        {
            foreach (var barrackProcessor in _barracksProcessors.Values)
            {
                barrackProcessor.Dispose();
            }
            _barracksProcessors.Clear();
            _subscriptions.Dispose();
        }

        public void Tick()
        {
            foreach (var processor in _barracksProcessors.Values)
            {
                processor.Tick();
            }
        }
    }

    public class BarrackBuildingProcessor : ITickable, IDisposable
    {
        private readonly BuildingModel _building;
        private readonly BarracksBuildingFunctionSo _function;
        private readonly BattleSystemModel _battleModel;
        private readonly BattleManager _battleManager;
        
        private readonly List<BattleUnitBase> _spawnedUnits = new();
        private readonly CompositeDisposable _subscriptions = new();

        private float _timer;
        private bool _isActive = true;

        public BarrackBuildingProcessor(BuildingModel building, BarracksBuildingFunctionSo function, BattleSystemModel battleModel, BattleManager battleManager)
        {
            _building = building;
            _function = function;
            _battleModel = battleModel;
            _battleManager = battleManager;
        }

        public void Tick()
        {
            if (_isActive && _battleModel.IsInBattle.Value)
            {
                _timer += Time.deltaTime;
                
                var funcData = _function.BarrackFunctionLevels[_building.Level.Value];
                
                if (funcData.MaxUnits <= _spawnedUnits.Count)
                {
                    return;
                }
                
                if (_timer >= funcData.SpawnTime)
                {
                    _timer = 0;

                    var createdUnits = _battleManager.PlayerUnitCreate(funcData.ProduceUnits, GetBuildingEnterPosition());
                    foreach (var unit in createdUnits)
                    {
                        unit.OnDiedObservable.Subscribe(OnDied).AddTo(_subscriptions);
                        _spawnedUnits.Add(unit);
                    }
                }
            }
            else if (_battleModel.IsInBattle.Value == false)
            {
                _timer = 0;
                
                for (var i = _spawnedUnits.Count - 1; i >= 0; i--)
                {
                    var desiredPosition = GetBuildingEnterPosition();
                    var unit = _spawnedUnits[i];
                    
                    unit.StartPosition.Value = desiredPosition;
                    
                    if ((unit.CurrentPosition - desiredPosition).sqrMagnitude < 1f)
                    {
                        _battleManager.Despawn(unit);
                        _spawnedUnits.RemoveAt(i);
                    }
                }
            }

            return;

            void OnDied(BattleUnitBase unit)
            {
                _spawnedUnits.Remove(unit);
            }
        }

        private Vector3 GetBuildingEnterPosition()
        {
            return _building.WorldPosition.Value + Vector3.forward * _building.Config.Size.Y * 0.5f;
        }
        
        public void Stop()
        {
            _isActive = false;
        }

        public void Dispose()
        {
            foreach (var unit in _spawnedUnits)
            {
                _battleManager.Despawn(unit);
            }
            _subscriptions.Dispose();
            _spawnedUnits.Clear();
        }
    }
}