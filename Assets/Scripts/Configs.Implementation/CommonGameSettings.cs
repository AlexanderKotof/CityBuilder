using System;
using System.Collections.Generic;
using Newtonsoft.Json;
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
}