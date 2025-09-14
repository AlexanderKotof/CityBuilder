using CityBuilder.BuildingSystem;

namespace ResourcesSystem
{
    public record StorageIncreaseUnit(ResourceStorageBuildingFunction Function, BuildingModel Building)
    {
        public ResourceStorageBuildingFunction Function { get; } = Function;
        public BuildingModel Building { get; } = Building;
    }
}