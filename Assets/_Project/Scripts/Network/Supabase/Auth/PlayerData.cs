using System;
using Newtonsoft.Json;
using Postgrest.Attributes;
using Postgrest.Models;

namespace CityBuilder.Network.SupabaseApi
{
    [Table("players"), Serializable]
    public class PlayerData : BaseModel
    {
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("display_name")]
        public string display_name { get; set; }
        [JsonProperty("level")]
        public int level { get; set; }
        [JsonProperty("score")]
        public int score { get; set; }
        [JsonProperty("created_at")]
        public DateTime created_at { get; set; }
        [JsonProperty("last_login")]
        public DateTime last_login { get; set; }
    }
}