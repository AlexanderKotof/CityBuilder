using CityBuilder.BuildingSystem;
using Configs.Implementation.Buildings.Functions;

namespace GameSystems.Implementation.PopulationFeature
{
    public record AvailableHouseholdIncreaseUnit(HouseHoldsIncreaseBuildingFunctionSO Function,  BuildingModel Building)
    {
        public HouseHoldsIncreaseBuildingFunctionSO Function { get; } = Function;
        public BuildingModel Building { get; } = Building;
    }
}