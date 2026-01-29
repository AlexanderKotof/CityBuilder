using UnityEngine.Serialization;

namespace Configs.Scriptable.Buildings.Functions
{
    public class ResourceStorageBuildingFunctionSo : BuildingFunctionSo
    {
        [FormerlySerializedAs("StorageCapacityIncrease")]
        public int _storageCapacityIncrease;
        [FormerlySerializedAs("PerBuildingLevelGrow")]
        public int _perBuildingLevelGrow;
    }
}