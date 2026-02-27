using System;
using CityBuilder.Configs.Implementation.Common;
using UnityEngine;

namespace CityBuilder.Configs.Scriptable.Buildings.Functions
{
    [CreateAssetMenu(fileName = nameof(ResourceProductionBuildingFunctionSo), menuName = ConfigsMenuName.BuildingFunctionsMenuName + nameof(ResourceProductionBuildingFunctionSo))]
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