using CityBuilder.BuildingSystem;
using ResourcesSystem;
using UnityEngine;

namespace Configs.Schemes
{
    public class BuildingConfigScheme : ConfigBase
    {
        public string Name;
        public string AssetKey;
        public Vector2Int Size = Vector2Int.one;

        public bool IsMovable = true;

        public ResourceConfig[] RequiredResources;

        public ConfigReference<BuildingFunction>[] BuildingFunctions { get; set;}
    }
}