using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Configs.Implementation.Buildings;
using Configs.Implementation.Buildings.Functions;

namespace CityBuilder.BuildingSystem
{
    [Obsolete]
    public static class BuildingConfigSchemeExtensions
    {
        public static bool TryGetProducingResourcesFunction(this BuildingConfigScheme bc,
            [NotNullWhen(true)] out ResourceProductionBuildingFunction production)
        {
            return TryGetBuildingFunction(bc, out production);
        }

        public static bool TryGetHouseholdsCapacityFunction(this BuildingConfigScheme bc,
            [NotNullWhen(true)] out HouseHoldsIncreaseBuildingFunction production)
        {
            return TryGetBuildingFunction(bc, out production);
        }

        public static bool TryGetResourceStorageCapacityFunction(this BuildingConfigScheme bc,
            [NotNullWhen(true)] out ResourceStorageBuildingFunction production)
        {
            return TryGetBuildingFunction(bc, out production);
        }
        
        public static bool TryGetBuildingFunction<T>(this BuildingConfigScheme bc, [NotNullWhen(true)] out T function)
            where T : class, IBuildingFunction
        {
            if (bc.BuildingFunctions == null)
            {
                function = default(T);
                return false;
            }

            function = bc.BuildingFunctions.FirstOrDefault(value => value.Value is T)?.Value as T;
            return function != default(T);
        }
    }
}