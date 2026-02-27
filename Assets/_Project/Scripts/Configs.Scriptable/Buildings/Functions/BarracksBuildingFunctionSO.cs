using System;
using CityBuilder.Configs.Scriptable.Battle;
using UnityEngine;

namespace CityBuilder.Configs.Scriptable.Buildings.Functions
{
    /// <summary>
    /// Spawns units during invasion
    /// </summary>
    [CreateAssetMenu(fileName = nameof(BarracksBuildingFunctionSo), menuName = ConfigsMenuName.BuildingFunctionsMenuName + nameof(BarracksBuildingFunctionSo))]
    public class BarracksBuildingFunctionSo : BuildingFunctionSo
    {
        [field: SerializeField]
        public BarrackFunctionLevelConfig[] BarrackFunctionLevels { get; private set; }
    }

    [Serializable]
    public class BarrackFunctionLevelConfig
    {
        [field: SerializeField]
        public BattleUnitConfigSO[] ProduceUnits { get; private set; }
        
        [field: SerializeField]
        public float SpawnTime { get; private set; }
        
        [field: SerializeField]
        public int MaxUnits { get; private set; }
    }
}