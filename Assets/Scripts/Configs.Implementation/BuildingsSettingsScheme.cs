using System;
using CityBuilder.BuildingSystem;
using Newtonsoft.Json;
using ResourcesSystem;
using UnityEngine;

namespace Configs.Schemes
{
    public class BuildingsSettingsScheme : IGameConfigScheme
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonProperty]
        public BuildingConfigScheme MainBuildingConfig { get; set; } = new BuildingConfigScheme()
        {
            Name = "Main Building",
            AssetKey = "Cityhall",
            IsMovable = false,
            Size = new Size(2, 2),
            RequiredResources = new ResourceConfig[1]
            {
                new() { Type = ResourceType.Food, Amount = 1 }
            },
            BuildingFunctions = new ConfigReference<BuildingFunction>[]
            {
                new ConfigReference<BuildingFunction>()
            }
        };

        [JsonProperty]
        public BuildingConfigScheme[] BuildingConfigs { get; set; } = new BuildingConfigScheme[]
        {
            new()
            {
                Name = "FARM",
                AssetKey = "Some key",
                IsMovable = true,
                RequiredResources = new ResourceConfig[1]
                {
                    new() { Type = ResourceType.Food, Amount = 1 }
                },
                BuildingFunctions = new ConfigReference<BuildingFunction>[]
                {
                    new ConfigReference<BuildingFunction>()
                },
                Size = new Size(1, 1)
            }
        };

        public BuildingFunction[] BuildingFunctions { get; set; } = new BuildingFunction[]
        {
            new MainBuildingFunction()
            {

            },
            new ResourceStorageBuildingFunction()
            {

            },
            new ResourceProductionBuildingFunction()
            {
                RequireResourcesForProduction = new ResourceConfig[]
                {
                    new() { Type = ResourceType.Food, Amount = 5 },
                },
                ProduceResourcesByTick = new ResourceConfig[]
                {
                    new() { Type = ResourceType.Food, Amount = 5 },
                }
            },
            new HouseHoldsIncreaseBuildingFunction
            {
                AvailableHouseholdsIncrease = 5,
                PerBuildingLevelGrow = 5
            },
            new MercenaryBuildingFunction()
            {

            }
        };
    }
}