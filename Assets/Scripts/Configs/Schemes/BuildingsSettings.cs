using System.Collections.Generic;
using Newtonsoft.Json;

namespace Configs.Schemes
{
    public class BuildingsSettings : IGameConfigScheme
    {
        [JsonProperty]
        public string MainBuildingId {  get; set; } = "MainBuilding_232";
        
        [JsonProperty]
        public string value { get; set; } = "abfdawaa";
        
        [JsonProperty]
        public SomeInnerData[] SomeData { get; set; } = new SomeInnerData[2]
        {
            new SomeInnerData()
            {
                value1 = "222",
                value2 = 333,
            },
            new SomeInnerData()
            {
                value1 = "22233",
                value2 = 3323,
            }
        };

        [JsonProperty]
        public Dictionary<int, SomeInnerData> SomeDataDictionary { get; set; } = new Dictionary<int, SomeInnerData>()
        {
            { 1, new SomeInnerData() }
        };
    }
}