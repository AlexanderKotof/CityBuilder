using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Configs.Scriptable.Buildings;
using Configs.Scriptable.Buildings.Functions;

namespace BuildingSystem.Extensions
{
    public static class BuildingConfigExtensions
    {
        public static bool TryGetProducingResourcesFunction(this BuildingConfigSo bc,
            [NotNullWhen(true)] out ResourceProductionBuildingFunctionSo production)
        {
            return TryGetBuildingFunction(bc, out production);
        }

        public static bool TryGetHouseholdsCapacityFunction(this BuildingConfigSo bc,
            [NotNullWhen(true)] out HouseHoldsIncreaseBuildingFunctionSo production)
        {
            return TryGetBuildingFunction(bc, out production);
        }

        public static bool TryGetResourceStorageCapacityFunction(this BuildingConfigSo bc,
            [NotNullWhen(true)] out ResourceStorageBuildingFunctionSo production)
        {
            return TryGetBuildingFunction(bc, out production);
        }
        
        public static bool TryGetBuildingFunction<T>(this BuildingConfigSo bc, [NotNullWhen(true)] out T function)
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