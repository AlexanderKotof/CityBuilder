using System;
using Configs.Implementation.Buildings.Functions;
using Configs.Schemes;
using Configs.Schemes.BattleSystem;
using Newtonsoft.Json;
using ResourcesSystem;

namespace Configs.Implementation.Buildings
{
    public class BuildingsSettingsScheme : IGameConfigScheme
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public BuildingConfigScheme MainBuildingConfig { get; set; }
        public BuildingConfigScheme[] BuildingConfigs { get; set; }
        public BuildingFunction[] BuildingFunctions { get; set; }

        public BuildingsSettingsScheme()
        {
            MainBuildingConfig = new BuildingConfigScheme()
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
            
            BuildingConfigs = new BuildingConfigScheme[]
            {
                new()
                {
                    Name = "FARM",
                    AssetKey = "Farm",
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
            
            BuildingFunctions = new BuildingFunction[]
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
                new MercenaryTrainingBuildingFunction()
                {
                    CanPurchase = new ConfigReference<BattleUnitConfig>[]
                    {
                        new ConfigReference<BattleUnitConfig>()
                    },
                },
                new MercenaryContainingFunction()
                {
                    Amount = 5,
                }
            };
        }
    }
}