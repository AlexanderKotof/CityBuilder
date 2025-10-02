using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Configs.Schemes
{
    public class CommonGameSettings : IGameConfigScheme
    {
        [JsonProperty]
        public float StartGoldGS { get; set; } = 10;
        
        [JsonProperty]
        public float StartGoldGPS { get; set; } = 101;
        
        [JsonProperty]
        public float EndGoldG { get; set; } = 1011;
        
        [JsonProperty]
        public string value { get; set; } = "abfda";
        
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}