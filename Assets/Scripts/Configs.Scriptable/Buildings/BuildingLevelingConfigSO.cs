using UnityEngine;

namespace Configs.Scriptable.Buildings
{
    public class BuildingLevelingConfigSo : ScriptableObject, IConfigBase
    {
        [field: SerializeField]
        public int StartLevel { get; private set; } = 0;
        
        [field: SerializeField]
        public int MaxLevel { get; private set; } = 3;
        
        [field: SerializeField]
        public bool LevelUpOnStack { get; private set; } = true;
    }
}