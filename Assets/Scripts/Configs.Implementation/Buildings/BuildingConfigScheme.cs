using System.Numerics;
using Configs.Implementation.Buildings.Functions;
using Configs.Schemes;
using ResourcesSystem;
using UnityEngine;

namespace Configs.Implementation.Buildings
{
    public class BuildingConfigScheme : ConfigBase
    {
        public string Name;
        public string AssetKey;
        public float Health;
        
        public Size Size = new Size(1, 1);

        public bool IsMovable = true;

        public ResourceConfig[] RequiredResources;

        public ConfigReference<BuildingFunction>[] BuildingFunctions { get; set;}
    }
}