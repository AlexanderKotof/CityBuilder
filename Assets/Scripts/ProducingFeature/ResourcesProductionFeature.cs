using System.Collections.Generic;
using CityBuilder.BuildingSystem;
using CityBuilder.Dependencies;
using ResourcesSystem;
using UnityEngine;

namespace ProducingFeature
{
    public class ResourcesProductionFeature : GameSystemBase
    {
        private readonly BuildingsModel _buildingsModel;
        
        public ProductionModel ProductionModel { get; } 
        
        private readonly Dictionary<BuildingModel, IResourceProducer> _buildingProducersMap = new();
        private readonly object _buildingsManager;
        private readonly GameTimeSystem.GameTimeSystem _gameTimeSystem;

        public ResourcesProductionFeature(IDependencyContainer conatiner) : base(conatiner)
        {
            _buildingsModel = conatiner.Resolve<BuildingsModel>();
            _buildingsManager = conatiner.Resolve<BuildingManager>();
            _gameTimeSystem = conatiner.Resolve<GameTimeSystem.GameTimeSystem>();
            
            ProductionModel = new(conatiner.Resolve<PlayerResourcesModel>());
        }
        public override void Init()
        {
            _buildingsModel.Buildings.SubscribeAdd(OnBuildingAdded);
            _buildingsModel.Buildings.SubscribeRemove(OnBuildingRemoved);

            foreach (var building in _buildingsModel.Buildings)
            {
                OnBuildingAdded(building);
            }
            
            _gameTimeSystem.NewDayStarted += OnNewDayStarted;
        }

        public override void Deinit()
        {
            _buildingsModel.Buildings.UnsubscribeAdd(OnBuildingAdded);
            _buildingsModel.Buildings.UnsubscribeRemove(OnBuildingRemoved);

            foreach (var building in _buildingsModel.Buildings)
            {
                OnBuildingRemoved(building);
            }
            
            _gameTimeSystem.NewDayStarted -= OnNewDayStarted;
        }


        public override void Update()
        {
            
        }

        private void OnBuildingAdded(BuildingModel building)
        {
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
            Debug.Log("Tick");
            ProductionModel.Tick();
        }
    }
}