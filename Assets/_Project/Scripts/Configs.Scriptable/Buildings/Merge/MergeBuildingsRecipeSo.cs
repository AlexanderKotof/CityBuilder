using CityBuilder.Configs.Implementation.Common;
using UnityEngine;

namespace CityBuilder.Configs.Scriptable.Buildings.Merge
{
    /// <summary>
    /// Рецепты для кастом мерджа зданий
    /// </summary>
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