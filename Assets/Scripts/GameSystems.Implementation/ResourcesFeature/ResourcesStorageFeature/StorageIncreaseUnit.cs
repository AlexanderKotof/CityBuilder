using Configs.Scriptable.Buildings.Functions;
using GameSystems.Implementation.BuildingSystem.Domain;

namespace GameSystems.Implementation.ResourcesStorageFeature
{
    public record StorageIncreaseUnit(ResourceStorageBuildingFunctionSo Function, BuildingModel Building)
    {
        public ResourceStorageBuildingFunctionSo Function { get; } = Function;
        public BuildingModel Building { get; } = Building;
    }
}