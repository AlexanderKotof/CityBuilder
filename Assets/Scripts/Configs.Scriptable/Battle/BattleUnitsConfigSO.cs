using UnityEngine;

namespace Configs.Scriptable
{
    [CreateAssetMenu(fileName = nameof(BattleUnitsConfigSO), menuName = ConfigsMenuName.BattleMenuName + nameof(BattleUnitsConfigSO))]
    public class BattleUnitsConfigSO : ScriptableObject, IGameConfig
    {
        [field: SerializeField]
        public BattleUnitConfigSO[] PlayerUnitsConfigs { get; set; }
        
        [field: SerializeField]
        public BattleUnitConfigSO[] EnemiesConfigs { get; set; }
        
        [field: SerializeField]
        public string BattleUiAssetKey { get; set; }
        
        [field: SerializeField]
        public BattleUnitConfigSO DefaultBuildingUnit { get; set; }
        
        [field: SerializeField]
        public BattleUnitConfigSO MainBuildingUnit { get; set; }
    }
}