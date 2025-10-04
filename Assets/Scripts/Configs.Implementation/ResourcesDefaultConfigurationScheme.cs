using System;
using ResourcesSystem;

namespace Configs.Schemes
{
    public class ResourcesDefaultConfigurationScheme : IGameConfigScheme
    {
        public ResourceConfig[] Resources { get; set; }

        public int DefaultCapacity = 1000;
        public Guid Id { get; set; } = Guid.NewGuid();

        public ResourcesDefaultConfigurationScheme()
        {
            Resources = new ResourceConfig[]
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
        }
    }
}