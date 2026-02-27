using UnityEngine;

namespace CityBuilder.Configs.Scriptable.Buildings.Functions
{
    [CreateAssetMenu(fileName = nameof(HouseHoldsIncreaseBuildingFunctionSo), menuName = ConfigsMenuName.BuildingFunctionsMenuName + nameof(HouseHoldsIncreaseBuildingFunctionSo))]
    public class HouseHoldsIncreaseBuildingFunctionSo : BuildingFunctionSo
    {
        public int AvailableHouseholdsIncreaseBase;
        public int[] PerBuildingLevel;
    }
}