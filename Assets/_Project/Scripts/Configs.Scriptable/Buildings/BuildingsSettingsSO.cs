using UnityEngine;

namespace CityBuilder.Configs.Scriptable.Buildings
{
    [CreateAssetMenu(fileName = nameof(BuildingsSettingsSo), menuName = ConfigsMenuName.BuildingsMenuName + nameof(BuildingsSettingsSo))]
    public class BuildingsSettingsSo : ScriptableObject, IGameConfig
    {
        [field: SerializeField]
        public BuildingConfigSo MainBuildingConfig { get; private  set; }
        [field: SerializeField]
        public BuildingConfigSo[] BuildingConfigs { get; private set; }
    }
}