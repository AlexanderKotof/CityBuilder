using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CityBuilder.Configs.Scriptable.Buildings;
using CityBuilder.Configs.Scriptable.Buildings.Functions;
using CityBuilder.GameSystems.Implementation.BuildingSystem.Domain;
using UnityEngine;

namespace CityBuilder.GameSystems.Implementation.BuildingSystem.Extensions
{
    public static class BuildingConfigExtensions
    {
        public static bool TryGetProducingResourcesFunction(this BuildingConfigSo bc,
            [NotNullWhen(true)] out ResourceProductionBuildingFunctionSo function)
        {
            return TryGetBuildingFunction(bc, out function);
        }

        public static bool TryGetHouseholdsCapacityFunction(this BuildingConfigSo bc,
            [NotNullWhen(true)] out HouseHoldsIncreaseBuildingFunctionSo function)
        {
            return TryGetBuildingFunction(bc, out function);
        }

        public static bool TryGetResourceStorageCapacityFunction(this BuildingConfigSo bc,
            [NotNullWhen(true)] out ResourceStorageBuildingFunctionSo function)
        {
            return TryGetBuildingFunction(bc, out function);
        }
        
        public static bool TryGetAttackFunction(this BuildingConfigSo bc,
            [NotNullWhen(true)] out AttackBuildingFunctionSo function)
        {
            return TryGetBuildingFunction(bc, out function);
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

        public static Vector3 GetBuildingCenterVisualPosition(this BuildingModel building)
        {
            var center = building.WorldPosition.Value;
            var size = building.Config.Size;
            center += new Vector3(size.X, (size.X + size.Y) * 0.5f, size.Y) * 0.5f;
            return center;
        }
    }
}