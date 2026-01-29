using BuildingSystem;
using Configs.Scriptable.Buildings.Functions;

namespace GameSystems.Implementation.ResourcesStorageFeature
{
    public record StorageIncreaseUnit(ResourceStorageBuildingFunctionSo Function, BuildingModel Building)
    {
        public ResourceStorageBuildingFunctionSo Function { get; } = Function;
        public BuildingModel Building { get; } = Building;
    }
}