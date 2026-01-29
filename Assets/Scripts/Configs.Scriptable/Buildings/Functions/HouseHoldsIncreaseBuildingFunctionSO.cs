using UnityEngine.Serialization;

namespace Configs.Scriptable.Buildings.Functions
{
    public class HouseHoldsIncreaseBuildingFunctionSo : BuildingFunctionSo
    {
        [FormerlySerializedAs("AvailableHouseholdsIncrease")]
        public int _availableHouseholdsIncrease;

        [FormerlySerializedAs("PerBuildingLevelGrow")]
        public int _perBuildingLevelGrow;
        //ToDo: how to calculate capacity or someth by building level?
    }
}