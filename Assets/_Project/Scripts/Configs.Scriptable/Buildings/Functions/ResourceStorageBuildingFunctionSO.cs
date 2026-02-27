using UnityEngine;

namespace CityBuilder.Configs.Scriptable.Buildings.Functions
{
    [CreateAssetMenu(fileName = nameof(ResourceStorageBuildingFunctionSo), menuName = ConfigsMenuName.BuildingFunctionsMenuName + nameof(ResourceStorageBuildingFunctionSo))]
    public class ResourceStorageBuildingFunctionSo : BuildingFunctionSo
    {
        public int StorageCapacityIncreaseBase;
        public int[] PerBuildingLevel;
    }
}