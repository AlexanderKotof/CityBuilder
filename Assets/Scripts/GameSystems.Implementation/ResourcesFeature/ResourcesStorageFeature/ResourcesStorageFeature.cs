using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CityBuilder.BuildingSystem;
using CityBuilder.Dependencies;
using Configs.Implementation.Buildings.Functions;
using ResourcesSystem;
using VContainer.Unity;

namespace GameSystems.Implementation.ResourcesStorageFeature
{
    public class ResourcesStorageFeature : IInitializable, IDisposable
    {
        private readonly PlayerResourcesModel _playerResourcesStorage;
        private readonly BuildingsModel _buildingsModel;
        
        private readonly Dictionary<BuildingModel, StorageIncreaseUnit> _storageIncreaseUnits = new();

        public ResourcesStorageFeature(PlayerResourcesModel resourcesModel, BuildingsModel buildingsModel)
        {
            _playerResourcesStorage = resourcesModel;
            _buildingsModel = buildingsModel;
        }

        public void Initialize()
        {
            _buildingsModel.Buildings.SubscribeAdd(OnBuildingAdded);
            _buildingsModel.Buildings.SubscribeRemove(OnBuildingRemoved);
        }

        public void Dispose()
        {
            _buildingsModel.Buildings.UnsubscribeAdd(OnBuildingAdded);
            _buildingsModel.Buildings.UnsubscribeRemove(OnBuildingRemoved);
        }
        
        private void OnBuildingAdded(BuildingModel building)
        {
            if (building.Config.TryGetResourceStorageCapacityFunction(out ResourceStorageBuildingFunctionSO storageIncrease) == false)
            {
                return;
            }
            
            var storageIncreaseUnit = new StorageIncreaseUnit(storageIncrease, building);
            _storageIncreaseUnits.Add(building, storageIncreaseUnit);
            
            building.Level.Subscribe(OnBuildingLevelChanged);
            
            UpdateStorageCapacity();
        }

        private void OnBuildingRemoved(BuildingModel building)
        {
            if (_storageIncreaseUnits.Remove(building, out var storageIncreaseUnit))
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