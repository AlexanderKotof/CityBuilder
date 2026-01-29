using Configs.Implementation.Common;
using ResourcesSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Configs.Scriptable
{
    [CreateAssetMenu(fileName = nameof(ResourcesDefaultConfigurationSo), menuName = ConfigsMenuName.MenuName + nameof(ResourcesDefaultConfigurationSo))]
    public class ResourcesDefaultConfigurationSo : ScriptableObject, IGameConfig
    {
        [field: SerializeField]
        public ResourceConfig[] StartResources { get; set; } = new ResourceConfig[]
        {
            new ResourceConfig()
            {
                Type = ResourceType.Food,
                Amount = 100
            },
            new ResourceConfig()
            {
                Type = ResourceType.Wood,
                Amount = 100
            },
            new ResourceConfig()
            {
                Type = ResourceType.Rock,
                Amount = 100
            },
            new ResourceConfig()
            {
                Type = ResourceType.Metal,
                Amount = 100
            },
            new ResourceConfig()
            {
                Type = ResourceType.Gold,
                Amount = 100
            },
        };

        [FormerlySerializedAs("DefaultCapacity")]
        [SerializeField]
        public int _defaultCapacity = 1000;
    }
}