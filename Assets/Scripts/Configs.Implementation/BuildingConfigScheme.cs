using CityBuilder.BuildingSystem;
using ResourcesSystem;
using UnityEngine;

namespace Configs.Schemes
{
    public class BuildingConfigScheme : ConfigBase
    {
        public string Name;
        public string AssetKey;
        public Size Size = new Size(1, 1);

        public bool IsMovable = true;

        public ResourceConfig[] RequiredResources;

        public ConfigReference<BuildingFunction>[] BuildingFunctions { get; set;}
    }
}