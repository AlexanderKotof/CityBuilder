using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ResourcesSystem;
using UnityEngine;

namespace Configs.Schemes
{
    public class CommonGameSettings : IGameConfigScheme
    {
        public string SelectorAssetReferenceKey { get; set; } = "cursor";
        
        [JsonProperty]
        public float StartGoldGS { get; set; } = 10;
        
        [JsonProperty]
        public float StartGoldGPS { get; set; } = 101;
        
        [JsonProperty]
        public float EndGoldG { get; set; } = 1011;
        
        [JsonProperty]
        public string value { get; set; } = "abfda";
        
        public Guid Id { get; set; } = Guid.NewGuid();
        public int InteractionRaycastLayerMask { get; set; } = 32;
    }

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