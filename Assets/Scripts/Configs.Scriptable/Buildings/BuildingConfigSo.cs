using Configs.Implementation.Common;
using Configs.Scriptable.Battle;
using UnityEngine;
using UnityEngine.Serialization;

namespace Configs.Scriptable.Buildings
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