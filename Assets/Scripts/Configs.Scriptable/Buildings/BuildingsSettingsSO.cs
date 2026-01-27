using Configs.Schemes;
using UnityEngine;

namespace Configs.Scriptable
{
    [CreateAssetMenu(fileName = nameof(BuildingsSettingsSO), menuName = ConfigsMenuName.BuildingsMenuName + nameof(BuildingsSettingsSO))]
    public class BuildingsSettingsSO : ScriptableObject, IConfigBase
    {
        [field: SerializeField]
        public BuildingConfigSO MainBuildingConfig { get; private  set; }
        [field: SerializeField]
        public BuildingConfigSO[] BuildingConfigs { get; private set; }
        
        //IS IT REQUIRED NOW?
        [field: SerializeField]
        public BuildingFunctionSO[] AllBuildingFunctions { get; private set; }
    }
}