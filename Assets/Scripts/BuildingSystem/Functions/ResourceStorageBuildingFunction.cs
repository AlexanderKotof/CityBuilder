using ResourcesSystem;

namespace CityBuilder.BuildingSystem
{
    public class ResourceStorageBuildingFunction : BuildingFunction
    {
        public int StorageCapacityIncrease { get; set;}
        public int PerBuildingLevelGrow { get; set;}
    }
}