using System;
using System.Diagnostics.CodeAnalysis;
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

    public static class BuildingConfigExtensions
    {
        public static bool TryGetProducingResourcesFunction(this BuildingConfig bc, [NotNullWhen(true)] out ResourceProductionBuildingFunction production)
        {
            production = null;
            foreach (var function in bc.BuildingFunctions)
            {
                if (function is ResourceProductionBuildingFunction buildingFunction)
                {
                    production = buildingFunction;
                    return true;
                }
            }

            return false;
        }
    }
}