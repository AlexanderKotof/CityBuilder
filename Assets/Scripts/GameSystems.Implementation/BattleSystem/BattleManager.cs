using System;
using System.Collections.Generic;
using System.Linq;
using Configs.Schemes.BattleSystem;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameSystems.Implementation.BattleSystem
{
    public class BattleManager
    {
        public BattleSystemModel BattleSystemModel { get; }
        
        private readonly BattleUnitsConfigScheme _battleUnitsConfigScheme;
        private readonly BattleUnitsProcessor _battleUnitsProcessor;

        private readonly Dictionary<Guid, BattleUnitConfig> _battleUnitConfigsMap;

        public BattleManager(BattleSystemModel battleSystemModel, BattleUnitsConfigScheme battleUnitsConfigScheme)
        {
            BattleSystemModel = battleSystemModel;
            _battleUnitsConfigScheme = battleUnitsConfigScheme;
            
            _battleUnitsProcessor = new BattleUnitsProcessor(battleSystemModel);
            
            _battleUnitConfigsMap = battleUnitsConfigScheme.Configs.ToDictionary(config => config.Id);
            _battleUnitConfigsMap.AddRange(battleUnitsConfigScheme.EnemiesConfigs.ToDictionary(config => config.Id));
            _battleUnitConfigsMap.TryAdd(battleUnitsConfigScheme.DefaultBuildingUnit.Id, battleUnitsConfigScheme.DefaultBuildingUnit);
            _battleUnitConfigsMap.TryAdd(battleUnitsConfigScheme.MainBuildingUnit.Id, battleUnitsConfigScheme.MainBuildingUnit);
        }
        
        public void Update()
        {
            _battleUnitsProcessor.Update();
            
            bool isInBattle = BattleSystemModel.Enemies.Count > 0;
            if (isInBattle != BattleSystemModel.IsInBattle.Value)
            {
                BattleSystemModel.IsInBattle.Value = isInBattle;
            }
        }
        
        public void PlayerUnitCreate(IEnumerable<Guid> guids)
        {
            foreach (var guid in guids)
            {
                if (_battleUnitConfigsMap.TryGetValue(guid, out var battleUnitConfig) == false)
                {
                    Debug.LogWarning($"Battle unit config not found {guid}");
                    continue;
                }

                Vector3 position = new Vector3(5, 0, 5);
                var unit = SpawnUnit(battleUnitConfig, position);
                BattleSystemModel.AddPlayerUnit(unit);
            }
        }

        public void InvasionBegins(InvasionData data)
        {
            foreach (var invader in data.Invaders)
            {
                for (int e = 0; e < invader.Amount; e++)
                {
                    if (_battleUnitConfigsMap.TryGetValue(invader.UnitConfigId, out var battleUnitConfig) == false)
                    {
                        Debug.LogWarning($"Battle unit config not found {invader.UnitConfigId}");
                        continue;
                    }

                    Vector3 position = GetEncounterPosition();
                    var unit = SpawnUnit(battleUnitConfig, position);
                    BattleSystemModel.AddEnemyUnit(unit);
                }
            }
        }

        private BattleUnitBase SpawnUnit(BattleUnitConfig config, Vector3 position)
        {
            var unitModel = new BattleUnitBase(config, 1, position);

            unitModel.OnUnitDied += OnUnitDied;
            
            return unitModel;
        }

        private Vector3 GetEncounterPosition()
        {
            var position = Random.onUnitSphere * 10;
            position.y = 0;
            return position;
        }

        private void OnUnitDied(IBattleUnit unit)
        {
            unit.OnUnitDied -= OnUnitDied;
            
            Debug.Log($"Unit {unit.RuntimeId} died!");
        }
    }
}