using System;
using System.Collections.Generic;
using System.Linq;
using Configs.Schemes.BattleSystem;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameSystems.Implementation.BattleSystem
{
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
            _battleUnitConfigsMap.TryAdd(battleUnitsConfigScheme.DefaultBuildingUnit.Id, battleUnitsConfigScheme.DefaultBuildingUnit);
            _battleUnitConfigsMap.TryAdd(battleUnitsConfigScheme.MainBuildingUnit.Id, battleUnitsConfigScheme.MainBuildingUnit);
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
                bool canAttack = unit.CanAttack;
                if (canAttack)
                {
                    SelectTarget(unit, unit.AttackModel!);
                }

                if (unit.CanMove)
                {
                    ProcessMove(unit);
                }

                if (canAttack)
                {
                    ProcessAttack(unit, unit.AttackModel!);
                }
            }
        }

        private void ProcessAttack(BattleUnitModel unit, UnitAttackModel attackModel)
        {
            if (attackModel.Target.Value == null)
            {
                return;
            }
            
            var sqrDistance = GetSqrDistanceToTarget(unit, attackModel);
            bool distanceCheck = sqrDistance <= unit.Config.AttackRange * unit.Config.AttackRange;
            bool timingCheck = attackModel.LastAttackTime.Value < Time.timeSinceLevelLoad + GetAttacksPerSecond(unit);
            
            if (distanceCheck && timingCheck)
            {
                //TODO: begin attack check timing and successfulness of
                
                attackModel.LastAttackTime.Value = Time.timeSinceLevelLoad;

                var target = attackModel.Target.Value;
                var damage = unit.Config.Damage;
                
                target.TakeDamage(damage);
                
                //TODO: On damage dealed
            }
        }

        private static float GetAttacksPerSecond(BattleUnitModel unit)
        {
            return 1 / unit.Config.AttackSpeed;
        }

        private void ProcessMove(BattleUnitModel unit)
        {
            if (unit.ThisTransform.Value == null ||
                unit.CurrentPosition == unit.DesiredPosition.Value)
            {
                return;
            }
            
            var deltaTime = Time.deltaTime;
            var impulse = Vector3.ClampMagnitude( unit.DesiredPosition.Value - unit.CurrentPosition, 1f) * unit.Config.MoveSpeed * deltaTime;
            
            unit.ThisTransform.Value.position += impulse;
        }

        private void SelectTarget(BattleUnitModel unit, UnitAttackModel attackModel)
        {
            bool isEnemy = BattleUnitsModel.Enemies.Contains(unit);
            if (isEnemy)
            {
                var target = SelectNearUnitOf(unit, 
                    BattleUnitsModel.PlayerUnits.AppendMany(BattleUnitsModel.PlayerBuildings));
                attackModel.SetTarget(target);
            }
            else
            {
                var target = SelectNearUnitOf(unit, BattleUnitsModel.Enemies);
                attackModel.SetTarget(target);
            }

            UpdateDesiredPosition(unit, attackModel);
        }

        private static void UpdateDesiredPosition(BattleUnitModel unit, UnitAttackModel attackModel)
        {
            Vector3 desiredPosition;
            if (attackModel.Target.Value != null)
            {
                var normalizedDelta = (unit.CurrentPosition - attackModel.Target.Value.CurrentPosition).normalized;
                desiredPosition = attackModel.Target.Value.CurrentPosition - normalizedDelta * unit.Config.AttackRange;
            }
            else
            {
                desiredPosition = unit.StartPosition.Value;
            }
            unit.DesiredPosition.Set(desiredPosition);
        }

        private float GetSqrDistanceToTarget(BattleUnitModel unit, UnitAttackModel attackModel)
        {
            if (attackModel.Target.Value == null)
            {
                return float.MaxValue;
            }

            return Vector3.SqrMagnitude(unit.CurrentPosition - attackModel.Target.Value.CurrentPosition);
        }

        [CanBeNull]
        private static IBattleUnit SelectNearUnitOf(IBattleUnit unit, IEnumerable<IBattleUnit> otherUnits)
        {
            IBattleUnit target = null;
            float minDistance = float.MaxValue;

            foreach (var playerUnit in otherUnits)
            {
                var sqrDistance = Vector3.SqrMagnitude(unit.CurrentPosition - playerUnit.CurrentPosition);
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
            _battleUnits.Add(unitModel.RuntimeId, unitModel);

            unitModel.OnUnitDied += OnUnitDied;
            
            return unitModel;
        }
        
        private void OnUnitDied(IBattleUnit unit)
        {
            unit.OnUnitDied -= OnUnitDied;

            _battleUnits.Remove(unit.RuntimeId);

            //BattleUnitsModel.RemoveUnit(unit);
        }
    }
}