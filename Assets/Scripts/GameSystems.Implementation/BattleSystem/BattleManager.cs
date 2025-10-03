using System;
using System.Collections.Generic;
using Configs.Schemes.BattleSystem;
using UnityEngine;

namespace GameSystems.Implementation.BattleSystem
{
    public class BattleManager
    {
        public BattleUnitsModel BattleUnitsModel { get; }
        
        private readonly Dictionary<Guid, BattleUnitModel> _battleUnits = new();

        public BattleManager(BattleUnitsModel battleUnitsModel)
        {
            BattleUnitsModel = battleUnitsModel;
        }

        public void EncounterBegins(object encounterData)
        {
            
        }

        public void SpawnUnit(BattleUnitConfig config, Vector3 position)
        {
            var unitModel = new BattleUnitModel(config, 1, position);
            _battleUnits.Add(unitModel.Id, unitModel);
            
            
        }
    }
}