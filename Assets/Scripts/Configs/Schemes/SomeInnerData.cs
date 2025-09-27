using Newtonsoft.Json;

namespace Configs.Schemes
{
    public class SomeInnerData
    {
        [JsonProperty]
        public string value1 { get; set; } = "abfdawaa";
        
        [JsonProperty]
        public int value2 { get; set; } = 2;
    }
}