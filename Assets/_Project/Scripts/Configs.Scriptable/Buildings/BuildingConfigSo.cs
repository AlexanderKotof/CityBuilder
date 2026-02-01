using CityBuilder.Configs.Implementation.Common;
using CityBuilder.Configs.Scriptable.Battle;
using CityBuilder.Configs.Scriptable.Buildings.Functions;
using CityBuilder.Configs.Scriptable.Buildings.Leveling;
using UnityEngine;

namespace CityBuilder.Configs.Scriptable.Buildings
{
    [CreateAssetMenu(fileName = nameof(BuildingConfigSo), menuName = ConfigsMenuName.BuildingsMenuName + nameof(BuildingConfigSo))]
    public class BuildingConfigSo : ScriptableObject, IConfigBase
    {
        public string Name;
        public string AssetKey;
        public Size Size = new Size(1, 1);
        public bool IsMovable = true;
        public ResourceConfig[] RequiredResources;
        public BuildingFunctionSo[] BuildingFunctions;
        public BattleUnitConfigSO? UnitConfig;
        public BuildingLevelingConfigSo BuildingLevelingConfig;
    }
}