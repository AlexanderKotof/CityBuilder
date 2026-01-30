using System;
using Configs.Implementation.Common;

namespace Configs.Scriptable.Buildings.Functions
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