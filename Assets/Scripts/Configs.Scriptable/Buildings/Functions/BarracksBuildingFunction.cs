using Configs.Schemes;
using Configs.Schemes.BattleSystem;
using Configs.Scriptable;

namespace Configs.Implementation.Buildings.Functions
{
    /// <summary>
    /// Spawns units during invasion
    /// </summary>
    public class BarracksBuildingFunctionSO : BuildingFunctionSO
    {
        public BattleUnitConfigSO[] ProduceUnits;

        //TODO: train rate in battle
    }
}