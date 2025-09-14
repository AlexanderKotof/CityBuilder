using CityBuilder.BuildingSystem;

namespace PeopleFeature
{
    public record AvailableHouseholdIncreaseUnit(HouseHoldsIncreaseBuildingFunction Function,  BuildingModel Building)
    {
        public HouseHoldsIncreaseBuildingFunction Function { get; } = Function;
        public BuildingModel Building { get; } = Building;
    }
}