using System.Diagnostics.CodeAnalysis;

namespace CityBuilder.BuildingSystem
{
    public static class BuildingConfigExtensions
    {
        public static bool TryGetProducingResourcesFunction(this BuildingConfig bc, [NotNullWhen(true)] out ResourceProductionBuildingFunction production)
        {
            production = null;

            if (bc.BuildingFunctions == null)
            {
                return false;
            }
            
            foreach (var function in bc.BuildingFunctions)
            {
                if (function is ResourceProductionBuildingFunction buildingFunction)
                {
                    production = buildingFunction;
                    return true;
                }
            }

            return false;
        }
        
        public static bool TryGetHouseholdsCapacityFunction(this BuildingConfig bc, [NotNullWhen(true)] out HouseHoldsIncreaseBuildingFunction production)
        {
            production = null;
            
            if (bc.BuildingFunctions == null)
            {
                return false;
            }
            
            foreach (var function in bc.BuildingFunctions)
            {
                if (function is HouseHoldsIncreaseBuildingFunction buildingFunction)
                {
                    production = buildingFunction;
                    return true;
                }
            }

            return false;
        }
        
        public static bool TryGetResourceStorageCapacityFunction(this BuildingConfig bc, [NotNullWhen(true)] out ResourceStorageBuildingFunction production)
        {
            production = null;
            
            if (bc.BuildingFunctions == null)
            {
                return false;
            }
            
            foreach (var function in bc.BuildingFunctions)
            {
                if (function is ResourceStorageBuildingFunction buildingFunction)
                {
                    production = buildingFunction;
                    return true;
                }
            }

            return false;
        }
    }
}