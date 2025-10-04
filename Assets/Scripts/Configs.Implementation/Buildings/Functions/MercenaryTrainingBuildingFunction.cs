using Configs.Schemes;
using Configs.Schemes.BattleSystem;

namespace Configs.Implementation.Buildings.Functions
{
    public class MercenaryTrainingBuildingFunction : BuildingFunction
    {
        public ConfigReference<BattleUnitConfig>[] CanPurchase { get; set; } 
    }
}