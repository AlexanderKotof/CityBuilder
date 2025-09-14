using ResourcesSystem;

namespace CityBuilder.BuildingSystem
{
    public static class BusinessFunctionsExtensions
    {
        public static int GetStorageIncreaseValue(this StorageIncreaseUnit storageUnit) => 
            storageUnit.Function.StorageCapacityIncrease + storageUnit.Building.Level.Value * storageUnit.Function.PerBuildingLevelGrow;
    }
}