using System;
using System.Collections.Generic;
using BuildingSystem;
using BuildingSystem.Extensions;
using Configs.Scriptable.Buildings.Functions;
using Cysharp.Threading.Tasks;
using GameSystems.Implementation.BattleSystem;
using ResourcesSystem;
using UniRx;
using VContainer.Unity;

namespace GameSystems.Implementation.ResourcesStorageFeature
{
    public class ResourcesStorageFeature : IInitializable, IDisposable
    {
        private readonly PlayerResourcesModel _playerResourcesStorage;
        private readonly BuildingsModel _buildingsModel;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        private readonly Dictionary<BuildingModel, StorageIncreaseUnit> _storageIncreaseUnits = new();

        public ResourcesStorageFeature(PlayerResourcesModel resourcesModel, BuildingsModel buildingsModel)
        {
            _playerResourcesStorage = resourcesModel;
            _buildingsModel = buildingsModel;
        }

        public void Initialize()
        {
            _buildingsModel.Buildings
                .SubscribeToCollection(OnBuildingAdded, OnBuildingRemoved).AddTo(_disposable);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
        
        private void OnBuildingAdded(BuildingModel building)
        {
            if (building.Config.TryGetResourceStorageCapacityFunction(out ResourceStorageBuildingFunctionSo storageIncrease) == false)
            {
                return;
            }
            
            var storageIncreaseUnit = new StorageIncreaseUnit(storageIncrease, building);
            _storageIncreaseUnits.Add(building, storageIncreaseUnit);
            
            building.Level.Subscribe(OnBuildingLevelChanged).AddTo(_disposable);
            
            UpdateStorageCapacity();
        }

        private void OnBuildingRemoved(BuildingModel building)
        {
            if (_storageIncreaseUnits.Remove(building, out _))
            {
                UpdateStorageCapacity();
            }
        }

        private void OnBuildingLevelChanged(int _) => UpdateStorageCapacity();

        private void UpdateStorageCapacity()
        {
            int capacity = _playerResourcesStorage.DefaultCapacity;
            foreach (var unit in _storageIncreaseUnits.Values)
            {
                capacity += unit.GetStorageIncreaseValue();
            }
            
            _playerResourcesStorage.UpdateCapacity(capacity);
        }

    }
}