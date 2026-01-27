using Configs.Schemes;
using UnityEngine;

namespace Configs.Scriptable
{
    [CreateAssetMenu(fileName = nameof(CommonGameSettingsSO), menuName = ConfigsMenuName.MenuName + nameof(CommonGameSettingsSO))]
    public class CommonGameSettingsSO : ScriptableObject, IConfigBase
    {
        [field: SerializeField]
        public string SelectorAssetReferenceKey { get; private set; } = "cursor";
        
        [field: SerializeField]
        public float StartGoldGS { get; private set; } = 10;
        
        [field: SerializeField]
        public float StartGoldGPS { get; private set; } = 101;
        
        [field: SerializeField]
        public float EndGoldG { get; private set; } = 1011;

        [field: SerializeField]
        public LayerMask InteractionRaycastLayerMask { get; private set; } = 32;
    }
}