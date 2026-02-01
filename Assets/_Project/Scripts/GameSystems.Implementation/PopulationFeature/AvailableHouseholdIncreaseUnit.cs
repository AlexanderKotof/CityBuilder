using CityBuilder.Configs.Scriptable.Buildings.Functions;
using CityBuilder.GameSystems.Implementation.BuildingSystem.Domain;

namespace CityBuilder.GameSystems.Implementation.PopulationFeature
{
    public record AvailableHouseholdIncreaseUnit(HouseHoldsIncreaseBuildingFunctionSo Function,  BuildingModel Building)
    {
        public HouseHoldsIncreaseBuildingFunctionSo Function { get; } = Function;
        public BuildingModel Building { get; } = Building;
    }
}