using Configs.Schemes;
using Configs.Schemes.BattleSystem;

namespace Configs.Implementation.Buildings.Functions
{
    /// <summary>
    /// Spawns units during invasion
    /// </summary>
    public class BarracksBuildingFunction : BuildingFunction
    {
        public ConfigReference<BattleUnitConfig>[] CanPurchase { get; set; }
        
        //TODO: train rate in battle
    }
}