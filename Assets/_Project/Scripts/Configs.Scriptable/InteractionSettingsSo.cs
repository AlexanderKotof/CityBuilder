using UnityEngine;

namespace CityBuilder.Configs.Scriptable
{
    [CreateAssetMenu(fileName = nameof(InteractionSettingsSo), menuName = ConfigsMenuName.MenuName + nameof(InteractionSettingsSo))]
    public class InteractionSettingsSo : ScriptableObject, IGameConfig
    {
        [field: SerializeField]
        public LayerMask InteractionRaycastLayerMask { get; private set; } = 32;
        
        [field: SerializeField]
        public float StartDragDelay { get; private set; } = 0.2f;
        
        [field: SerializeField]
        public float StartDragThreshold { get; private set; } = 10;
        
        [field: SerializeField]
        public float CameraDragThreshold { get; private set; } = 1;
    }
}