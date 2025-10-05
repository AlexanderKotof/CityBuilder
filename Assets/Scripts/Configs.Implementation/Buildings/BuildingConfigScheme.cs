using System.Numerics;
using Configs.Implementation.Buildings.Functions;
using Configs.Schemes;
using Configs.Schemes.BattleSystem;
using GameSystems.Implementation.BattleSystem;
using ResourcesSystem;
using UnityEngine;

namespace Configs.Implementation.Buildings
{
    public class BuildingConfigScheme : ConfigBase
    {
        public string Name;
        public string AssetKey;

        public Size Size = new Size(1, 1);

        public bool IsMovable = true;

        public ResourceConfig[] RequiredResources;

        public ConfigReference<BuildingFunction>[] BuildingFunctions { get; set;}
        
        public ConfigReference<BattleUnitConfig>? UnitConfig { get; set;} 
    }
}