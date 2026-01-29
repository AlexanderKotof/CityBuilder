using CityBuilder.BuildingSystem;
using Configs.Implementation.Buildings.Functions;

namespace GameSystems.Implementation.ResourcesStorageFeature
{
    public record StorageIncreaseUnit(ResourceStorageBuildingFunctionSO Function, BuildingModel Building)
    {
        public ResourceStorageBuildingFunctionSO Function { get; } = Function;
        public BuildingModel Building { get; } = Building;
    }
}