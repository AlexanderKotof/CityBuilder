using BuildingSystem;
using Configs.Scriptable.Buildings.Functions;

namespace GameSystems.Implementation.PopulationFeature
{
    public record AvailableHouseholdIncreaseUnit(HouseHoldsIncreaseBuildingFunctionSo Function,  BuildingModel Building)
    {
        public HouseHoldsIncreaseBuildingFunctionSo Function { get; } = Function;
        public BuildingModel Building { get; } = Building;
    }
}