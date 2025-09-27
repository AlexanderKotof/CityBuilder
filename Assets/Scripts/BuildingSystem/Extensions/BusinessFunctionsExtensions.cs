using GameSystems.Implementation.PopulationFeature;
using ResourcesSystem;

namespace CityBuilder.BuildingSystem
{
    public static class BusinessFunctionsExtensions
    {
        public static int GetStorageIncreaseValue(this StorageIncreaseUnit storageUnit) => 
            storageUnit.Function.StorageCapacityIncrease + (storageUnit.Building.Level.Value - 1) * storageUnit.Function.PerBuildingLevelGrow;
        
        public static int GetHouseholdIncreaseValue(this AvailableHouseholdIncreaseUnit unit) =>
            unit.Function.AvailableHouseholdsIncrease + (unit.Building.Level.Value - 1) * unit.Function.PerBuildingLevelGrow;
    }
}