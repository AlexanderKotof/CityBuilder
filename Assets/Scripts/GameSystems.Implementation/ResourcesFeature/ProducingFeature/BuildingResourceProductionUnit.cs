using System.Collections.Generic;
using Configs.Implementation.Common;
using Configs.Scriptable.Buildings.Functions;
using ResourcesSystem;

namespace GameSystems.Implementation.ProducingFeature
{
    public record BuildingResourceProductionUnit(ResourceProductionBuildingFunctionSo Function) : IResourceProducer
    {
        public ResourceProductionBuildingFunctionSo Function { get; } = Function;

        public bool CanProduce()
        {
            return true;
        }

        public IEnumerable<ResourceConfig> GetCosts()
        {
            return Function._requireResourcesForProduction;
        }

        public IEnumerable<ResourceConfig> GetProduction()
        {
            return Function._produceResourcesByTick;
        }

    }
}