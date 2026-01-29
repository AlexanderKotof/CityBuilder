using Configs.Implementation.Common;
using Configs.Scriptable.Battle;
using UnityEngine;
using UnityEngine.Serialization;

namespace Configs.Scriptable.Buildings
{
    [CreateAssetMenu(fileName = nameof(BuildingConfigSo), menuName = ConfigsMenuName.BuildingsMenuName + nameof(BuildingConfigSo))]
    public class BuildingConfigSo : ScriptableObject, IConfigBase
    {
        [FormerlySerializedAs("Name")]
        public string _name;
        [FormerlySerializedAs("AssetKey")]
        public string _assetKey;

        [FormerlySerializedAs("Size")]
        public Size _size = new Size(1, 1);

        [FormerlySerializedAs("IsMovable")]
        public bool _isMovable = true;
        
        [FormerlySerializedAs("RequiredResources")]
        public ResourceConfig[] _requiredResources;

        [FormerlySerializedAs("BuildingFunctions")]
        public BuildingFunctionSo[] _buildingFunctions;

        [FormerlySerializedAs("UnitConfig")]
        public BattleUnitConfigSO? _unitConfig;
    }
}