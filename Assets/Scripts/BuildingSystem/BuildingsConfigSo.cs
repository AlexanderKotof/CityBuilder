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

        public BuildingConfig GetConfigByName(string name)
        {
            return Configs.First(config => config.Name == name);
        }
    }

    public enum BuildingType
    {
        Storage,
        Production,
        Infantry,
    }

    [Serializable]
    public class BuildingConfig
    {
        public string Name;
        public GameObject Prefab;
        public Vector2Int Size = Vector2Int.one;

        public ResourceConfig[] RequiredResources;

        public BuildingFunction[] BuildingFunctions;
    }
}