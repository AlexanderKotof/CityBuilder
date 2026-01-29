using System;
using System.Collections.Generic;
using BuildingSystem;
using BuildingSystem.Extensions;
using GameSystems.Implementation.GameTime;
using VContainer.Unity;

namespace GameSystems.Implementation.ProducingFeature
{
    //ToDo: convert to production feature (can produce not only resources)
    // and add support of workers
    public class ResourcesProductionFeature : IInitializable, IDisposable
    {
        private readonly BuildingsModel _buildingsModel;
        private readonly ProductionModel _productionModel; 
        private readonly BuildingManager _buildingsManager;
        private readonly GameTimeSystem _gameTimeSystem;
        
        private readonly Dictionary<BuildingModel, IResourceProducer> _buildingProducersMap = new();

        public ResourcesProductionFeature(BuildingsModel buildingsModel, BuildingManager buildingManager, GameTimeSystem gameTimeSystem, ProductionModel productionModel)
        {
            _buildingsModel = buildingsModel;
            _buildingsManager = buildingManager;
            _gameTimeSystem = gameTimeSystem;
            _productionModel = productionModel;
            
        }
        public void Initialize()
        {
            _buildingsModel.Buildings.SubscribeAdd(OnBuildingAdded);
            _buildingsModel.Buildings.SubscribeRemove(OnBuildingRemoved);

            foreach (var building in _buildingsModel.Buildings)
            {
                OnBuildingAdded(building);
            }
            
            _gameTimeSystem.NewDayStarted += OnNewDayStarted;
        }

        public void Dispose()
        {
            _buildingsModel.Buildings.UnsubscribeAdd(OnBuildingAdded);
            _buildingsModel.Buildings.UnsubscribeRemove(OnBuildingRemoved);

            foreach (var building in _buildingsModel.Buildings)
            {
                OnBuildingRemoved(building);
            }
            
            _gameTimeSystem.NewDayStarted -= OnNewDayStarted;
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
            _productionModel.AddResourceProducer(producer);
            _buildingProducersMap.Add(building, producer);
        }

        private void OnBuildingRemoved(BuildingModel building)
        {
            if (_buildingProducersMap.Remove(building, out var producer) == false)
            {
                return;
            }

            _productionModel.RemoveResourceProducer(producer);
        }

        private void OnNewDayStarted(int _)
        {
            _productionModel.Tick();
        }
    }
}