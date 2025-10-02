namespace CityBuilder.BuildingSystem
{
    public class HouseHoldsIncreaseBuildingFunction : BuildingFunction
    {
        public int AvailableHouseholdsIncrease { get; set;}
        public int PerBuildingLevelGrow { get; set;}
        //ToDo: how to calculate capacity or someth by building level?
    }
}