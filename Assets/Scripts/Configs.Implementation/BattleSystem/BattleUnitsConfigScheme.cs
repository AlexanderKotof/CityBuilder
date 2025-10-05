using System;
using Newtonsoft.Json;

namespace Configs.Schemes.BattleSystem
{
    public class BattleUnitConfig : ConfigBase
    {
        public string Name = "Unit";
        public float Health = 100;
        public float Damage = 5;
        public float AttackRange = 1;
        public float Defense = 0;
        public float MoveSpeed = 1;
        public float AttackSpeed = 1;
        public string AssetKey = "Unit";
    }
    
    public class BattleUnitsConfigScheme : ConfigBase, IGameConfigScheme
    {
        [JsonProperty]
        public BattleUnitConfig[] Configs { get; set; }
        
        [JsonProperty]
        public BattleUnitConfig DefaultBuildingUnit { get; set; }
        
        [JsonProperty]
        public BattleUnitConfig MainBuildingUnit { get; set; }
        
        public BattleUnitsConfigScheme()
        {
            Configs  = new [] { new BattleUnitConfig() };
            
            DefaultBuildingUnit = new BattleUnitConfig();
            MainBuildingUnit = new();
        }
    }
}