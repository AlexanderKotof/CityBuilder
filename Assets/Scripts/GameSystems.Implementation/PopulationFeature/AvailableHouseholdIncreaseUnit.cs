using CityBuilder.BuildingSystem;
using Configs.Implementation.Buildings.Functions;

namespace GameSystems.Implementation.PopulationFeature
{
    public record AvailableHouseholdIncreaseUnit(HouseHoldsIncreaseBuildingFunction Function,  BuildingModel Building)
    {
        public HouseHoldsIncreaseBuildingFunction Function { get; } = Function;
        public BuildingModel Building { get; } = Building;
    }
}