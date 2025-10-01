using System.Collections.Generic;
using System.Threading.Tasks;
using CityBuilder.BuildingSystem;
using CityBuilder.Dependencies;
using GameSystems;
using ResourcesSystem;
using UnityEngine;

namespace ProducingFeature
{
    //ToDo: convert to production feature (can produce not only resources)
    // and add support of workers
    public class ResourcesProductionFeature : GameSystemBase
    {
        private readonly BuildingsModel _buildingsModel;
        
        public ProductionModel ProductionModel { get; } 
        
        private readonly Dictionary<BuildingModel, IResourceProducer> _buildingProducersMap = new();
        private readonly BuildingManager _buildingsManager;
        private readonly GameTimeSystem.GameTimeSystem _gameTimeSystem;

        public ResourcesProductionFeature(IDependencyContainer container) : base(container)
        {
            _buildingsModel = container.Resolve<BuildingsModel>();
            _buildingsManager = container.Resolve<BuildingManager>();
            _gameTimeSystem = container.Resolve<GameTimeSystem.GameTimeSystem>();
            
            ProductionModel = new(container.Resolve<PlayerResourcesModel>());
        }
        public override Task Init()
        {
            _buildingsModel.Buildings.SubscribeAdd(OnBuildingAdded);
            _buildingsModel.Buildings.SubscribeRemove(OnBuildingRemoved);

            foreach (var building in _buildingsModel.Buildings)
            {
                OnBuildingAdded(building);
            }
            
            _gameTimeSystem.NewDayStarted += OnNewDayStarted;
            
            return Task.CompletedTask;
        }

        public override Task Deinit()
        {
            _buildingsModel.Buildings.UnsubscribeAdd(OnBuildingAdded);
            _buildingsModel.Buildings.UnsubscribeRemove(OnBuildingRemoved);

            foreach (var building in _buildingsModel.Buildings)
            {
                OnBuildingRemoved(building);
            }
            
            _gameTimeSystem.NewDayStarted -= OnNewDayStarted;
            
            return Task.CompletedTask;
        }

        private void OnBuildingAdded(BuildingModel building)
        {
            if (_buildingProducersMap.ContainsKey(building))
            {
                return;
            }
            
            if (building.Config.TryGetProducingResourcesFunction(out var producingResourcesFunction) == false)
            {
                return;
            }

            var producer = new BuildingResourceProductionUnit(producingResourcesFunction);
            ProductionModel.AddResourceProducer(producer);
            _buildingProducersMap.Add(building, producer);
        }

        private void OnBuildingRemoved(BuildingModel building)
        {
            if (_buildingProducersMap.Remove(building, out var producer) == false)
            {
                return;
            }

            ProductionModel.RemoveResourceProducer(producer);
        }

        private void OnNewDayStarted(int _)
        {
            ProductionModel.Tick();
        }
    }
}