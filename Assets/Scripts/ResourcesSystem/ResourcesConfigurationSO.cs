using UnityEngine;

namespace ResourcesSystem
{
    [CreateAssetMenu(fileName = "ResourcesConfiguration", menuName = "ScriptableObjects/ResourcesConfiguration", order = 1)]
    public class ResourcesConfigurationSO : ScriptableObject
    {
        public ResourceConfig[] Resources;
    }
}