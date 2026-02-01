using UnityEngine;

namespace CityBuilder.Configs.Scriptable.Buildings.Merge
{
    public class MergeFeatureConfigurationSo : ScriptableObject, IGameConfig
    {
        [field: SerializeField]
        public bool Enabled { get; private set; } = true;
        
        [field: SerializeField]
        public int MergeBuildingsCountForLevelUp { get; private set; } = 3;
        
        [field: SerializeField]
        public MergeBuildingsRecipeSo[] MergeRecipes { get; private set; }
    }
}