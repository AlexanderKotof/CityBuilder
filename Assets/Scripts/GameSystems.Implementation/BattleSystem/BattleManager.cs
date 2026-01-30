using System;
using System.Collections.Generic;
using System.Linq;
using Configs.Scriptable;
using Configs.Scriptable.Battle;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameSystems.Implementation.BattleSystem
{
    public class BattleManager
    {
        private readonly BattleSystemModel _battleSystemModel;
        private readonly BattleUnitsConfigSO _battleUnitsConfigScheme;
        private readonly BattleUnitsProcessor _battleUnitsProcessor;

        //private readonly Dictionary<string, BattleUnitConfigSO> _battleUnitConfigsMap;

        public BattleManager(BattleSystemModel battleSystemModel, BattleUnitsConfigSO battleUnitsConfigScheme, BattleUnitsProcessor battleUnitsProcessor)
        {
            _battleSystemModel = battleSystemModel;
            _battleUnitsConfigScheme = battleUnitsConfigScheme;
            _battleUnitsProcessor = battleUnitsProcessor;
            
            // _battleUnitConfigsMap = battleUnitsConfigScheme.PlayerUnitsConfigs.ToDictionary(config => config.Id);
            // _battleUnitConfigsMap.AddRange(battleUnitsConfigScheme.EnemiesConfigs.ToDictionary(config => config.Id));
            // _battleUnitConfigsMap.TryAdd(battleUnitsConfigScheme.DefaultBuildingUnit.Id, battleUnitsConfigScheme.DefaultBuildingUnit);
            // _battleUnitConfigsMap.TryAdd(battleUnitsConfigScheme.MainBuildingUnit.Id, battleUnitsConfigScheme.MainBuildingUnit);
        }
        
        public void Update()
        {
            _battleUnitsProcessor.Update();
            
            bool isInBattle = _battleSystemModel.Enemies.Count > 0;
            if (isInBattle != _battleSystemModel.IsInBattle.Value)
            {
                _battleSystemModel.IsInBattle.Value = isInBattle;
            }
        }
        
        public void PlayerUnitCreate(IEnumerable<BattleUnitConfigSO> configs)
        {
            foreach (var config in configs)
            {
                Vector3 position = new Vector3(5, 0, 5);
                var unit = SpawnUnit(config, position);
                _battleSystemModel.AddPlayerUnit(unit);
            }
        }

        public void InvasionBegins(InvasionData data)
        {
            foreach (var invader in data.Invaders)
            {
                for (int e = 0; e < invader.Amount; e++)
                {
                    var battleUnitConfig = invader.config;
                    Vector3 position = GetEncounterPosition();
                    var unit = SpawnUnit(battleUnitConfig, position);
                    _battleSystemModel.AddEnemyUnit(unit);
                }
            }
        }

        private BattleUnitBase SpawnUnit(BattleUnitConfigSO config, Vector3 position)
        {
            var unitModel = new BattleUnitBase(config, 1, position);

            unitModel.OnUnitDied += OnUnitDied;
            
            return unitModel;
        }

        private Vector3 GetEncounterPosition()
        {
            var position = Random.onUnitSphere * 5;
            position.y = 0;
            return position;
        }

        private void OnUnitDied(IBattleUnit unit)
        {
            unit.OnUnitDied -= OnUnitDied;
            
            Debug.Log($"Unit {unit.Config.Name} ({unit.RuntimeId.ToString().Substring(0, 4)}..) died!");
        }
    }
}