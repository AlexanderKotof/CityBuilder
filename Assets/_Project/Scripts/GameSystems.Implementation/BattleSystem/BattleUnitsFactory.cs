using CityBuilder.Configs.Implementation.Common;
using CityBuilder.Configs.Scriptable.Battle;
using CityBuilder.GameSystems.Implementation.BattleSystem.Domain.Units;
using CityBuilder.GameSystems.Implementation.BuildingSystem.Domain;
using CityBuilder.GameSystems.Implementation.BuildingSystem.Extensions;
using UnityEngine;

namespace CityBuilder.GameSystems.Implementation.BattleSystem
{
    public class BattleUnitsFactory
    {
        private readonly BattleUnitsConfigSO _config;

        public BattleUnitsFactory(BattleUnitsConfigSO config)
        {
            _config = config;
        }
        
        public BattleUnitBase CreateMainBuildingUnit(BuildingModel building)
        {
            var config = building.Config.UnitConfig != null ? building.Config.UnitConfig : _config.MainBuildingUnit;
            return CreateBattleUnitFromBuilding(building, config);
        }

        public BattleUnitBase CreateBattleUnitFromBuilding(BuildingModel building)
        {
            if (building.Config.TryGetAttackFunction(out var function))
            {
                return CreateBattleUnitFromBuilding(building, function.BattleUnitPerLevel[Mathf.Min(function.BattleUnitPerLevel.Length - 1, building.Level.Value)]); 
            }
            
            var config = building.Config.UnitConfig != null ? building.Config.UnitConfig : _config.DefaultBuildingUnit;
            return CreateBattleUnitFromBuilding(building, config);
        }

        public bool TryLevelUpBuildingUnit(BuildingModel building, BattleUnitBase unit)
        {
            if (building.IsMaxLevel)
                return false;
            
            if (building.Config.TryGetAttackFunction(out var function))
            {
                unit.ApplyConfig(function.BattleUnitPerLevel[Mathf.Min(function.BattleUnitPerLevel.Length - 1, building.Level.Value)]);
                return true;
            }

            if (true)
            {
                //TODO: common level up
            }
            
            return false;
        }
        
        private BattleUnitBase CreateBattleUnitFromBuilding(BuildingModel building, BattleUnitConfigSO config)
        {
            return new BattleUnitBase(
                config,
                building.Level.Value,
                building.WorldPosition.Value,
                building.ThisTransform,
                new Vector3((float)building.Config.Size.X/2, 0, (float)building.Config.Size.Y/2));
        }
    }
}