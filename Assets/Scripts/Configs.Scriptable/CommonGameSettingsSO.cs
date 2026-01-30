using UnityEngine;

namespace Configs.Scriptable
{
    [CreateAssetMenu(fileName = nameof(CommonGameSettingsSo), menuName = ConfigsMenuName.MenuName + nameof(CommonGameSettingsSo))]
    public class CommonGameSettingsSo : ScriptableObject, IGameConfig
    {
        [field: SerializeField]
        public string SelectorAssetReferenceKey { get; private set; } = "cursor";
        
        [field: SerializeField]
        public float StartGoldGS { get; private set; } = 10;
        
        [field: SerializeField]
        public float StartGoldGPS { get; private set; } = 101;
        
        [field: SerializeField]
        public float EndGoldG { get; private set; } = 1011;
    }
}