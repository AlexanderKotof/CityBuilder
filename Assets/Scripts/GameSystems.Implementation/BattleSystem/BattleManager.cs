using System;
using System.Collections.Generic;
using System.Linq;
using Configs.Schemes.BattleSystem;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameSystems.Implementation.BattleSystem
{
    public class EncounterData
    {
        public List<(Guid UnitConfigId, int Amount)> Encounters = new ();
    }
    
    public class BattleManager
    {
        public BattleUnitsModel BattleUnitsModel { get; }
        
        private readonly Dictionary<Guid, BattleUnitModel> _battleUnits = new();
        
        private readonly BattleUnitsConfigScheme _battleUnitsConfigScheme;
        private readonly Dictionary<Guid, BattleUnitConfig> _battleUnitConfigsMap;

        public BattleManager(BattleUnitsModel battleUnitsModel, BattleUnitsConfigScheme battleUnitsConfigScheme)
        {
            BattleUnitsModel = battleUnitsModel;
            _battleUnitsConfigScheme = battleUnitsConfigScheme;
            _battleUnitConfigsMap = battleUnitsConfigScheme.Configs.ToDictionary(config => config.Id);
        }

        public void EncounterBegins(EncounterData data)
        {
            foreach (var encounter in data.Encounters)
            {
                for (int e = 0; e < encounter.Amount; e++)
                {
                    if (_battleUnitConfigsMap.TryGetValue(encounter.UnitConfigId, out var battleUnitConfig) == false)
                    {
                        Debug.LogWarning($"Battle unit config not found {encounter.UnitConfigId}");
                        continue;
                    }

                    Vector3 position = GetEncounterPosition();
                    var unit = SpawnUnit(battleUnitConfig, position);
                    BattleUnitsModel.AddEnemyUnit(unit);
                }
            }
        }

        public void Update()
        {
            if (_battleUnits.Count == 0)
            {
                return;
            }

            foreach (var unit in _battleUnits.Values)
            {
                SelectTarget(unit);
                ProcessMove(unit);
                ProcessAttack(unit);
            }
        }

        private void ProcessAttack(BattleUnitModel unit)
        {
            if (unit.Target.Value == null)
            {
                return;
            }
            
            var sqrDistance = GetSqrDistanceToTarget(unit);
            bool distanceChack = sqrDistance <= unit.Config.AttackRange * unit.Config.AttackRange;
            bool timingCheck = unit.LastAttackTime.Value < Time.timeSinceLevelLoad + GetAttacksPerSecond(unit);
            
            if (distanceChack && timingCheck)
            {
                //TODO: begin attack check timing and successfulness of
                
                unit.LastAttackTime.Value = Time.timeSinceLevelLoad;

                var target = unit.Target.Value;
                var damage = unit.Config.Damage;
                
                target.TakeDamage(damage);
            }
        }

        private static float GetAttacksPerSecond(BattleUnitModel unit)
        {
            return 1 / unit.Config.AttackSpeed;
        }

        private void ProcessMove(BattleUnitModel unit)
        {
            if (unit.CurrentPosition.Value == unit.DesiredPosition.Value)
            {
                return;
            }
            
            var deltaTime = Time.deltaTime;
            var impulse = Vector3.ClampMagnitude( unit.DesiredPosition.Value - unit.CurrentPosition.Value, 1f) * unit.Config.MoveSpeed * deltaTime;
            
            unit.CurrentPosition.Value += impulse;
        }

        private void SelectTarget(BattleUnitModel unit)
        {
            bool isEnemy = BattleUnitsModel.Enemies.Contains(unit);
            if (isEnemy)
            {
                var target = SelectNearUnitOf(unit, BattleUnitsModel.PlayerUnits);
                unit.Target.Set(target);
            }
            else
            {
                var target = SelectNearUnitOf(unit, BattleUnitsModel.Enemies);
                unit.Target.Set(target);
            }

            UpdateDesiredPosition(unit);
        }

        private static void UpdateDesiredPosition(BattleUnitModel unit)
        {
            Vector3 desiredPosition;
            if (unit.Target.Value != null)
            {
                var normalizedDelta = (unit.CurrentPosition.Value - unit.Target.Value.CurrentPosition.Value).normalized;
                desiredPosition = unit.Target.Value.CurrentPosition.Value - normalizedDelta * unit.Config.AttackRange;
            }
            else
            {
                desiredPosition = unit.StartPosition.Value;
            }
            unit.DesiredPosition.Set(desiredPosition);
        }

        private float GetSqrDistanceToTarget(BattleUnitModel unit)
        {
            if (unit.Target.Value == null)
            {
                return float.MaxValue;
            }

            return Vector3.SqrMagnitude(
                unit.ThisTransform.Value.position - unit.Target.Value.ThisTransform.Value.position);
        }

        [CanBeNull]
        private static BattleUnitModel SelectNearUnitOf(BattleUnitModel unit, IEnumerable<BattleUnitModel> otherUnits)
        {
            BattleUnitModel target = null;
            float minDistance = float.MaxValue;

            foreach (var playerUnit in otherUnits)
            {
                var sqrDistance = Vector3.SqrMagnitude(unit.CurrentPosition.Value - playerUnit.CurrentPosition.Value);
                if (sqrDistance <= minDistance)
                {
                    minDistance = sqrDistance;
                    target = playerUnit;
                }
            }

            return target;
        }

        private Vector3 GetEncounterPosition()
        {
            var position = Random.onUnitSphere * 10;
            position.y = 0;
            return position;
        }

        public BattleUnitModel SpawnUnit(BattleUnitConfig config, Vector3 position)
        {
            var unitModel = new BattleUnitModel(config, 1, position);
            _battleUnits.Add(unitModel.Id, unitModel);
            return unitModel;
        }
    }
}