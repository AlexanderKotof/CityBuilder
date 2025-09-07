using System;
using System.Collections.Generic;
using System.Linq;
using CityBuilder.BuildingSystem;
using CityBuilder.Dependencies;
using ResourcesSystem;
using UnityEngine;

namespace ProducingFeature
{

    public class ProductionModel
    {
        private readonly List<IResourceProducer> _resourceProducers = new();
        private readonly ResourcesModel _playerResourcesModel;
        
        private readonly ResourcesModel _innerCostsModel = new();

        public ProductionModel(ResourcesModel playerResourcesModel)
        {
            _playerResourcesModel = playerResourcesModel;
        }

        public void AddResourceProducer(IResourceProducer producer)
        {
            _resourceProducers.Add(producer);
        }

        public void RemoveResourceProducer(IResourceProducer producer)
        {
            _resourceProducers.Remove(producer);
        }

        public void Tick()
        {
            var costs = GetDailyCostsRaw();
            UpdateResourceModel(_innerCostsModel, costs);
            
            Write("Costs for day is: {0}", _innerCostsModel.Resources);

            var hasDailyCosts = _playerResourcesModel.HasResources(_innerCostsModel);

            if (hasDailyCosts)
            {
                _playerResourcesModel.RemoveResources(_innerCostsModel);
                
                var production = GetDailyProductionRaw();
                UpdateResourceModel(_innerCostsModel, production);
                
                Write("Production for day is: {0}", _innerCostsModel.Resources);
                
                _playerResourcesModel.AddResources(_innerCostsModel);
            }
            else
            {
                //ToDo: some magic
            }
        }

        private void Write(string format, IEnumerable<ResourceModel> resources)
        {
            string resourceFormat = "{0}x{1}";
            string accumulate = string.Empty;
            Debug.Log(string.Format(format, 
                resources.Aggregate(accumulate, 
                    (current, resource) =>
                    {
                        return current + string.Format(resourceFormat, resource.Amount, resource.Id) +
                               Environment.NewLine;
                    })));
        }

        private void UpdateResourceModel(ResourcesModel resourcesModel, IEnumerable<ResourceConfig> newResources)
        {
            resourcesModel.Clear();
            foreach (var resource in newResources)
            {
                resourcesModel.AddResource(resource);
            }
        }

        private IEnumerable<ResourceConfig> GetDailyProductionRaw()
        {
            foreach (var producer in _resourceProducers)
            {
                if (producer.CanProduce() == false)
                {
                    continue;
                }

                foreach (var resource in producer.GetProduction())
                {
                    yield return resource;
                }
            }
        }
        
        private IEnumerable<ResourceConfig> GetDailyCostsRaw()
        {
            IEnumerable<ResourceConfig> dailyCostsRaw = 
                _resourceProducers.Where(producer =>
                    {
                        if (producer.CanProduce())
                        {
                            return true;
                        }
                        
                        Debug.Log("Cannot produce");
                        return false;
                    }).
                    Select(producer =>
                    {
                        Debug.Log("return costs " + producer.GetCosts().Count());
                        return producer.GetCosts();
                    }).
                SelectMany(resources =>
                    {
                        Debug.Log("selecting resources " + resources.Count());
                        return resources;
                    });
            return dailyCostsRaw;
        }
    }

    public interface IResourceProducer
    {
        bool CanProduce();
        IEnumerable<ResourceConfig> GetCosts();
        IEnumerable<ResourceConfig> GetProduction();
    }

    public record BuildingResourceProducer(ResourceProductionBuildingFunction Function) : IResourceProducer
    {
        public ResourceProductionBuildingFunction Function { get; } = Function;

        public bool CanProduce()
        {
            return true;
        }

        public IEnumerable<ResourceConfig> GetCosts()
        {
            return Function.RequireResourcesForProduction;
        }

        public IEnumerable<ResourceConfig> GetProduction()
        {
            return Function.ProduceResourcesByTick;
        }

    }
    
    public class ProducingFeature : GameSystemBase
    {
        private readonly BuildingsModel _buildingsModel;
        
        public ProductionModel ProductionModel { get; } 
        
        private readonly Dictionary<Building, IResourceProducer> _buildingProducersMap = new();
        private readonly object _buildingsManager;
        private readonly GameTimeSystem.GameTimeSystem _gameTimeSystem;

        public ProducingFeature(IDependencyContainer conatiner) : base(conatiner)
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

        private void OnBuildingAdded(Building building)
        {
            if (building.Config.TryGetProducingResourcesFunction(out var producingResourcesFunction) == false)
            {
                return;
            }

            var producer = new BuildingResourceProducer(producingResourcesFunction);
            ProductionModel.AddResourceProducer(producer);
            _buildingProducersMap.Add(building, producer);
        }

        private void OnBuildingRemoved(Building building)
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