using UnityEngine;

namespace Configs.Scriptable.Buildings
{
    /// <summary>
    /// Используется для мерджа зданий
    /// </summary>
    public class BuildingLevelingConfigSo : ScriptableObject, IConfigBase
    {
        [field: SerializeField]
        public int StartLevel { get; private set; } = 0;
        
        [field: SerializeField]
        public int MaxLevel { get; private set; } = 3;
    }
}