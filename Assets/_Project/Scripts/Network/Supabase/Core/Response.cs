using System;
using Newtonsoft.Json;

namespace Network.Supabase.Core
{
    [Serializable]
    public record Response
    {
        [JsonProperty("success")]
        public bool success { get; set; }
		
        [JsonProperty("message")]
        public string message { get; set; } = string.Empty;
		
        [JsonProperty("status")]
        public int status { get; set; } 
        
        [JsonProperty("error")]
        public string error { get; set; } = string.Empty;
    }

    [Serializable]
    public record Response<TPayload> : Response
    {
        [JsonProperty("payload")]
        public TPayload payload { get; set; } = default;
    }
}