using Configs.Implementation.Common;
using UnityEngine;

namespace Configs.Scriptable.Buildings
{
    /// <summary>
    /// Используется для кастомного апгрейда (главное здание)
    /// </summary>
    public class UpgradeableBuildingLevelingConfigSo : BuildingLevelingConfigSo
    {
        [field: SerializeField]
        public ResourceConfig[] UpgradeCost { get; private set; }
        
        [field: SerializeField]
        public int DaysPerLevel { get; private set; }
    }
}