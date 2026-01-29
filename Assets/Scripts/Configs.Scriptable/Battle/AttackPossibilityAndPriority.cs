namespace Configs.Scriptable.Battle
{
    public enum AttackPossibilityAndPriority
    {
        UnitsOnly = 0,
        BuildingsOnly = 1,
        
        MainBuildingOnly = 10,
        DefensiveBuildingsOnly = 11,
        
        UnitsThenBuildings = 20,
        UnitsThenMainBuildings = 21,
    }
}