using Configs.Scriptable.Buildings.Functions;
using GameSystems.Implementation.BuildingSystem.Domain;

namespace GameSystems.Implementation.PopulationFeature
{
    public record AvailableHouseholdIncreaseUnit(HouseHoldsIncreaseBuildingFunctionSo Function,  BuildingModel Building)
    {
        public HouseHoldsIncreaseBuildingFunctionSo Function { get; } = Function;
        public BuildingModel Building { get; } = Building;
    }
}