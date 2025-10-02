using CityBuilder.BuildingSystem;
using Configs.Implementation.Buildings.Functions;

namespace ResourcesSystem
{
    public record StorageIncreaseUnit(ResourceStorageBuildingFunction Function, BuildingModel Building)
    {
        public ResourceStorageBuildingFunction Function { get; } = Function;
        public BuildingModel Building { get; } = Building;
    }
}