using CityBuilder.Configs.Scriptable.Buildings.Functions;
using CityBuilder.GameSystems.Implementation.BuildingSystem.Domain;

namespace CityBuilder.GameSystems.Implementation.ResourcesFeature.ResourcesStorageFeature
{
    public record StorageIncreaseUnit(ResourceStorageBuildingFunctionSo Function, BuildingModel Building)
    {
        public ResourceStorageBuildingFunctionSo Function { get; } = Function;
        public BuildingModel Building { get; } = Building;
    }
}