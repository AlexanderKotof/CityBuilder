using CityBuilder.Configs.Scriptable.Battle;
using UnityEngine.Serialization;

namespace CityBuilder.Configs.Scriptable.Buildings.Functions
{
    /// <summary>
    /// Spawns units during invasion
    /// </summary>
    public class BarracksBuildingFunctionSo : BuildingFunctionSo
    {
        [FormerlySerializedAs("ProduceUnits")]
        public BattleUnitConfigSO[] _produceUnits;

        //TODO: train rate in battle
    }
}