using Configs.Implementation.Common;
using UnityEngine;

namespace Configs.Scriptable.Buildings
{
    public class MergeBuildingsRecipeSo : ScriptableObject, IConfigBase
    {
        [field: SerializeField]
        public BuildingConfigSo[] RequiredBuildings { get; private set; }
        
        [field: SerializeField]
        public ResourceConfig[] AdditionalCosts { get; private set; }
        
        [field: SerializeField]
        public BuildingConfigSo Product { get; private set; }
    }
}