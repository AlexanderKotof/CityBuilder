using System.Collections.Generic;
using CityBuilder.BuildingSystem;
using Newtonsoft.Json;
using ResourcesSystem;
using UnityEngine;

namespace Configs.Schemes
{
    public class BuildingConfigScheme
    {
        public string Name;
        public string AssetKey;
        public Vector2Int Size = Vector2Int.one;

        public bool IsMovable = true;

        public ResourceConfig[] RequiredResources;

        public IBuildingFunction[] BuildingFunctions;
    }
    
    public class OtherBuildingFunction : IBuildingFunction
    {
        public int ssss;
        public int sss;
        //ToDo: how to calculate capacity or someth by building level?
    }
    
    public class HouseHoldsIncreaseBuildingFunction : IBuildingFunction
    {
        public int AvailableHouseholdsIncrease;
        public int PerBuildingLevelGrow;
        //ToDo: how to calculate capacity or someth by building level?
    }
    
    public class BuildingsSettings : IGameConfigScheme
    {
        [JsonProperty]
        public string MainBuildingId {  get; set; } = "MainBuilding_232";
        
        [JsonProperty]
        public string value { get; set; } = "abfdawaa";

        [JsonProperty]
        public BuildingConfigScheme[] BuildingConfigs { get; set; } = new BuildingConfigScheme[]
        {
            new ()
            {
                Name = "FARM",
                AssetKey = "Some key",
                IsMovable = true,
                RequiredResources = new ResourceConfig[1]
                {
                    new () { Type = ResourceType.Food, Amount = 1 }
                },
                BuildingFunctions = new IBuildingFunction[]
                {
                    new HouseHoldsIncreaseBuildingFunction
                    {
                        AvailableHouseholdsIncrease = 5,
                        PerBuildingLevelGrow = 5
                    },
                    new OtherBuildingFunction
                    {
                        sss = 2,
                        ssss = 1,
                    }
                },
                Size = Vector2Int.one,
            }
        };

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