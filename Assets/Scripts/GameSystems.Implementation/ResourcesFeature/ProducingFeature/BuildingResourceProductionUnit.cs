using System.Collections.Generic;
using Configs.Implementation.Buildings.Functions;
using ResourcesSystem;

namespace GameSystems.Implementation.ProducingFeature
{
    public record BuildingResourceProductionUnit(ResourceProductionBuildingFunctionSO Function) : IResourceProducer
    {
        public ResourceProductionBuildingFunctionSO Function { get; } = Function;

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