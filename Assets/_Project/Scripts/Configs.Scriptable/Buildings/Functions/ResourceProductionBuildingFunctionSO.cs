using System;
using CityBuilder.Configs.Implementation.Common;

namespace CityBuilder.Configs.Scriptable.Buildings.Functions
{
    public class ResourceProductionBuildingFunctionSo : BuildingFunctionSo
    {
        public BuildingProductionByLevel[] ProductionsByBuildingLevel;
    }

    [Serializable]
    public class BuildingProductionByLevel
    {
        public ResourceConfig[] RequireResourcesForProduction;
        public ResourceConfig[] ProduceResourcesByTick;
    }
}