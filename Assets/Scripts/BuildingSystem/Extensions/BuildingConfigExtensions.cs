using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Configs.Implementation.Buildings.Functions;
using Configs.Scriptable;

namespace CityBuilder.BuildingSystem
{
    public static class BuildingConfigExtensions
    {
        public static bool TryGetProducingResourcesFunction(this BuildingConfigSO bc,
            [NotNullWhen(true)] out ResourceProductionBuildingFunctionSO production)
        {
            return TryGetBuildingFunction(bc, out production);
        }

        public static bool TryGetHouseholdsCapacityFunction(this BuildingConfigSO bc,
            [NotNullWhen(true)] out HouseHoldsIncreaseBuildingFunctionSO production)
        {
            return TryGetBuildingFunction(bc, out production);
        }

        public static bool TryGetResourceStorageCapacityFunction(this BuildingConfigSO bc,
            [NotNullWhen(true)] out ResourceStorageBuildingFunctionSO production)
        {
            return TryGetBuildingFunction(bc, out production);
        }
        
        public static bool TryGetBuildingFunction<T>(this BuildingConfigSO bc, [NotNullWhen(true)] out T function)
            where T : class, IBuildingFunction
        {
            if (bc.BuildingFunctions == null)
            {
                function = default(T);
                return false;
            }

            function = bc.BuildingFunctions.FirstOrDefault(value => value is T) as T;
            return function != default(T);
        }
    }
}