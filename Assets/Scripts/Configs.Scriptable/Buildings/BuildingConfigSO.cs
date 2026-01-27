using Configs.Schemes;
using ResourcesSystem;
using UnityEngine;

namespace Configs.Scriptable
{
    [CreateAssetMenu(fileName = nameof(BuildingConfigSO), menuName = ConfigsMenuName.BuildingsMenuName + nameof(BuildingConfigSO))]
    public class BuildingConfigSO : ScriptableObject, IConfigBase
    {
        public string Name;
        public string AssetKey;

        public Size Size = new Size(1, 1);

        public bool IsMovable = true;

        public ResourceConfig[] RequiredResources;

        public BuildingFunctionSO[] BuildingFunctions;

        public BattleUnitConfigSO? UnitConfig;
    }
}