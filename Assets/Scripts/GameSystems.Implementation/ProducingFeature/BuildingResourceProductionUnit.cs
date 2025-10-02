using System.Collections.Generic;
using CityBuilder.BuildingSystem;
using Configs.Implementation.Buildings.Functions;
using ResourcesSystem;

namespace ProducingFeature
{
    public record BuildingResourceProductionUnit(ResourceProductionBuildingFunction Function) : IResourceProducer
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
}