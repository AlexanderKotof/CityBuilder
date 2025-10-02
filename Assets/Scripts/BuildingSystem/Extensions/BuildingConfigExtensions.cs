using System.Diagnostics.CodeAnalysis;
using Configs.Schemes;

namespace CityBuilder.BuildingSystem
{
    public static class BuildingConfigExtensions
    {
        public static bool TryGetProducingResourcesFunction(this BuildingConfigScheme bc, [NotNullWhen(true)] out ResourceProductionBuildingFunction production)
        {
            production = null;

            if (bc.BuildingFunctions == null)
            {
                return false;
            }
            
            foreach (var function in bc.BuildingFunctions)
            {
                if (function.Value is ResourceProductionBuildingFunction buildingFunction)
                {
                    production = buildingFunction;
                    return true;
                }
            }

            return false;
        }
        
        public static bool TryGetHouseholdsCapacityFunction(this BuildingConfigScheme bc, [NotNullWhen(true)] out HouseHoldsIncreaseBuildingFunction production)
        {
            production = null;
            
            if (bc.BuildingFunctions == null)
            {
                return false;
            }
            
            foreach (var function in bc.BuildingFunctions)
            {
                if (function.Value is HouseHoldsIncreaseBuildingFunction buildingFunction)
                {
                    production = buildingFunction;
                    return true;
                }
            }

            return false;
        }
        
        public static bool TryGetResourceStorageCapacityFunction(this BuildingConfigScheme bc, [NotNullWhen(true)] out ResourceStorageBuildingFunction production)
        {
            production = null;
            
            if (bc.BuildingFunctions == null)
            {
                return false;
            }
            
            foreach (var function in bc.BuildingFunctions)
            {
                if (function.Value is ResourceStorageBuildingFunction buildingFunction)
                {
                    production = buildingFunction;
                    return true;
                }
            }

            return false;
        }
    }
}