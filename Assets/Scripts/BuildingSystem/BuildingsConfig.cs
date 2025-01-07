using System;
using System.Linq;
using UnityEngine;

namespace BuildingSystem
{
    [CreateAssetMenu(fileName = "BuildingsConfig", menuName = "BuildingsConfig")]
    public class BuildingsConfig : ScriptableObject
    {
        public BuildingConfig[] Configs;

        public BuildingConfig GetConfigByName(string name)
        {
            return Configs.First(config => config.Name == name);
        }
    }
    
    [Serializable]
    public class BuildingConfig
    {
        public string Name;
        public GameObject Prefab;
        public Vector2Int Size = Vector2Int.one;
        
        //ToDo building costs, etc.
    }
}
