using System.Collections.Generic;
using System.Threading.Tasks;
using CityBuilder.BuildingSystem;
using CityBuilder.Dependencies;
using Configs.Implementation.Buildings.Functions;
using GameSystems;

namespace ResourcesSystem
{
    public class ResourcesStorageFeature : GameSystemBase
    {
        private readonly PlayerResourcesModel _playerResourcesStorage;
        private readonly BuildingsModel _buildingsModel;
        
        private readonly Dictionary<BuildingModel, StorageIncreaseUnit> _storageIncreaseUnits = new();

        public ResourcesStorageFeature(IDependencyContainer container) : base(container)
        {
            _playerResourcesStorage = container.Resolve<PlayerResourcesModel>();
            _buildingsModel = container.Resolve<BuildingsModel>();
        }

        public override Task Init()
        {
            _buildingsModel.Buildings.SubscribeAdd(OnBuildingAdded);
            _buildingsModel.Buildings.SubscribeRemove(OnBuildingRemoved);
            return Task.CompletedTask;
        }
        
        public override Task Deinit()
        {
            _buildingsModel.Buildings.UnsubscribeAdd(OnBuildingAdded);
            _buildingsModel.Buildings.UnsubscribeRemove(OnBuildingRemoved);
            return Task.CompletedTask;
        }
        
        private void OnBuildingAdded(BuildingModel building)
        {
            if (building.Config.TryGetResourceStorageCapacityFunction(out ResourceStorageBuildingFunction storageIncrease) == false)
            {
                return;
            }
            
            var storageIncreaseUnit = new StorageIncreaseUnit(storageIncrease, building);
            _storageIncreaseUnits.Add(building, storageIncreaseUnit);
            
            building.Level.AddListener(OnBuildingLevelChanged);
            
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