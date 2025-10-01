using System;
using System.Linq;
using JetBrains.Annotations;
using ResourcesSystem;
using UnityEngine;

namespace CityBuilder.BuildingSystem
{

    [CreateAssetMenu(fileName = "BuildingsConfig", menuName = "BuildingsConfig")]
    public class BuildingsConfigSo : ScriptableObject
    {
        public BuildingConfig[] Configs;
        
        public BuildingConfig MainBuildingConfig;

        public BuildingConfig GetConfigByName(string name)
        {
            return Configs.First(config => config.Name == name);
        }
    }

    [Serializable]
    public class BuildingConfig
    {
        public string Name;
        public string AssetKey;
        public Vector2Int Size = Vector2Int.one;

        public bool IsMovable = true;

        public ResourceConfig[] RequiredResources;

        public BuildingFunction[] BuildingFunctions;
    }
}