using GameSystems.Implementation.PopulationFeature;
using GameSystems.Implementation.ResourcesStorageFeature;
using UnityEngine;

namespace GameSystems.Implementation.BuildingSystem.Extensions
{
    public static class BusinessFunctionsExtensions
    {
        public static int GetStorageIncreaseValue(this StorageIncreaseUnit storageUnit)
        {
            return storageUnit.Function.StorageCapacityIncreaseBase +
                   storageUnit.Function.PerBuildingLevel[Mathf.Min(storageUnit.Building.Level.Value, storageUnit.Function.PerBuildingLevel.Length)];
        }

        public static int GetHouseholdIncreaseValue(this AvailableHouseholdIncreaseUnit unit) =>
            unit.Function.AvailableHouseholdsIncreaseBase +
            unit.Function.PerBuildingLevel[Mathf.Min(unit.Building.Level.Value, unit.Function.PerBuildingLevel.Length)];

    }
}