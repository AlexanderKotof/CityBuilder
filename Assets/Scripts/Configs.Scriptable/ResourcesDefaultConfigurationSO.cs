using Configs.Implementation.Common;
using ResourcesSystem;
using UnityEngine;

namespace Configs.Scriptable
{
    [CreateAssetMenu(fileName = nameof(ResourcesDefaultConfigurationSO), menuName = ConfigsMenuName.MenuName + nameof(ResourcesDefaultConfigurationSO))]
    public class ResourcesDefaultConfigurationSO : ScriptableObject, IGameConfig
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

        [SerializeField]
        public int DefaultCapacity = 1000;
    }
}