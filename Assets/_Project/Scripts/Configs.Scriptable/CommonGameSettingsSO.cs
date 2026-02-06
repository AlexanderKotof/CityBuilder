using UnityEngine;

namespace CityBuilder.Configs.Scriptable
{
    [CreateAssetMenu(fileName = nameof(CommonGameSettingsSo), menuName = ConfigsMenuName.MenuName + nameof(CommonGameSettingsSo))]
    public class CommonGameSettingsSo : ScriptableObject, IGameConfig
    {
        [field: SerializeField]
        public string SelectorAssetReferenceKey { get; private set; } = "cursor";

        [field: SerializeField]
        public float CameraMovementSensitivity { get; private set; } = 0.1f;

        [field: SerializeField]
        public Vector3 MaxCameraPosition { get; private set; } = Vector3.one * 10f;
        [field: SerializeField]
        public Vector3 MinCameraPosition  { get; private set; } = Vector3.zero;
    }
}