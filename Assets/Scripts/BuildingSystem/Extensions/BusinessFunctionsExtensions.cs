using GameSystems.Implementation.PopulationFeature;
using GameSystems.Implementation.ResourcesStorageFeature;

namespace BuildingSystem.Extensions
{
    public static class BusinessFunctionsExtensions
    {
        public static int GetStorageIncreaseValue(this StorageIncreaseUnit storageUnit) => 
            storageUnit.Function._storageCapacityIncrease + (storageUnit.Building.Level.Value - 1) * storageUnit.Function._perBuildingLevelGrow;
        
        public static int GetHouseholdIncreaseValue(this AvailableHouseholdIncreaseUnit unit) =>
            unit.Function._availableHouseholdsIncrease + (unit.Building.Level.Value - 1) * unit.Function._perBuildingLevelGrow;
    }
}