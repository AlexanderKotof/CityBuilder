using System;
using System.Collections.Generic;
using System.Linq;
using ResourcesSystem;
using UnityEngine;

namespace ProducingFeature
{
    public class ProductionModel
    {
        private readonly List<IResourceProducer> _resourceProducers = new();
        private readonly PlayerResourcesModel _playerResourcesModel;
        
        private readonly ResourcesAmounts _innerCostsModel = new();

        public ProductionModel(PlayerResourcesModel playerResourcesModel)
        {
            _playerResourcesModel = playerResourcesModel;
        }

        public void AddResourceProducer(IResourceProducer producer) => _resourceProducers.Add(producer);

        public void RemoveResourceProducer(IResourceProducer producer) => _resourceProducers.Remove(producer);

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
                Debug.LogError("No production!!!");
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

        private void UpdateResourceModel(ResourcesAmounts resourcesModel, IEnumerable<ResourceConfig> newResources)
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
}