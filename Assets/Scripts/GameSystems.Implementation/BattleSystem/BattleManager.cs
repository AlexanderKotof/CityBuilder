using System;
using System.Collections.Generic;
using System.Linq;
using Configs.Schemes.BattleSystem;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameSystems.Implementation.BattleSystem
{
    public static class BattleRules
    {
        public const float MoveMagnitudeThreshold = 0.1f;
        
        public static float GetAttacksPerSecond(this IBattleUnit unit)
        {
            return 1 / unit.Config.AttackSpeed;
        }

        public static float GetRealMoveSpeed(this IBattleUnit unit)
        {
            return unit.Config.MoveSpeed;
        }
        
        public static float GetAttackRange(this IBattleUnit unit)
        {
            return unit.Config.AttackRange;
        }
        
        public static float GetAttackRangeSqr(this IBattleUnit unit)
        {
            return unit.GetAttackRange() * unit.GetAttackRange();
        }
    }
    
    
    public class BattleManager
    {
        public BattleUnitsModel BattleUnitsModel { get; }
        
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
        
        public void Update()
        {
            var enemyUnitsCount = BattleUnitsModel.Enemies.Count;

            if (enemyUnitsCount == 0)
            {
                foreach (var playerUnit in BattleUnitsModel.PlayerUnits)
                {
                    ProcessReturnToStart(playerUnit);
                }
                return;
            }

            // Process player units
            foreach (var playerUnit in BattleUnitsModel.PlayerUnits)
            {
                UpdateUnit(playerUnit, true);
            }
            
            // Process enemies
            foreach (var playerUnit in BattleUnitsModel.Enemies)
            {
                UpdateUnit(playerUnit, false);
            }

            // Process player buildings
            foreach (var buildingUnit in BattleUnitsModel.PlayerBuildings)
            {
                UpdateUnit(buildingUnit, true);
            }
        }

        private void ProcessReturnToStart(BattleUnitBase unit)
        {
            if (unit.CanMove)
            {
                var desiredPosition = unit.StartPosition.Value;
                unit.DesiredPosition.Set(desiredPosition);
                
                ProcessMove(unit);
            }
        }

        private void UpdateUnit(BattleUnitBase unit, bool isPlayer)
        {
            bool canAttack = unit.CanAttack;
            if (canAttack)
            {
                SelectTarget(unit, unit.AttackModel!, isPlayer);
            }

            if (unit.CanMove)
            {
                UpdateDesiredPosition(unit, unit.AttackModel!);
                ProcessMove(unit);
            }

            if (canAttack)
            {
                ProcessAttack(unit, unit.AttackModel!);
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
                BattleUnitsModel.AddPlayerUnit(unit);
            }
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
        
        //TODO: spawn player units
        public BattleUnitModel SpawnUnit(BattleUnitConfig config, Vector3 position)
        {
            var unitModel = new BattleUnitModel(config, 1, position);

            unitModel.OnUnitDied += OnUnitDied;
            
            return unitModel;
        }
        
        private void ProcessAttack(IBattleUnit unit, UnitAttackModel attackModel)
        {
            if (attackModel.Target.Value == null)
            {
                return;
            }
            
            var sqrDistance = GetSqrDistanceToTarget(unit, attackModel);
            bool distanceCheck = sqrDistance <= unit.GetAttackRangeSqr();
            bool timingCheck = attackModel.LastAttackTime.Value + 1 / unit.GetAttacksPerSecond() < Time.timeSinceLevelLoad;
            
            if (distanceCheck && timingCheck)
            {
                //TODO: begin attack check timing and successfulness of
                
                attackModel.LastAttackTime.Value = Time.timeSinceLevelLoad;

                var target = attackModel.Target.Value;
                var damage = unit.Config.Damage;
                
                Debug.LogError($"Unit {unit.RuntimeId} attacks {target.RuntimeId} w damage {damage}");
                
                target.TakeDamage(damage);
                
                //TODO: On damage dealed
            }
        }
        
        private float GetSqrDistanceToTarget(IBattleUnit unit, UnitAttackModel attackModel)
        {
            if (attackModel.Target.Value == null)
            {
                return float.MaxValue;
            }

            return Vector3.SqrMagnitude(unit.CurrentPosition - attackModel.Target.Value.CurrentPosition);
        }

        private void ProcessMove(BattleUnitBase unit)
        {
            if (unit.ThisTransform.Value == null ||
                unit.CurrentPosition == unit.DesiredPosition.Value)
            {
                return;
            }
            
            var deltaTime = Time.deltaTime;
            var impulse = unit.DesiredPosition.Value - unit.CurrentPosition;
            var delta = deltaTime * unit.GetRealMoveSpeed() * Vector3.ClampMagnitude(impulse, 1);
            unit.ThisTransform.Value.Translate(delta);
        }

        private void SelectTarget(IBattleUnit unit, UnitAttackModel attackModel, bool isPlayer)
        {
            if (isPlayer)
            {
                var target = SelectNearUnitOf(unit, BattleUnitsModel.Enemies);
                attackModel.SetTarget(target);
            }
            else
            {
                if (attackModel.Target.Value == null &&
                    BattleUnitsModel.PlayerUnits.Count == 0)
                {
                    IBattleUnit target = SelectNearUnitOf(unit, BattleUnitsModel.PlayerBuildings);
                    attackModel.SetTarget(target);
                    return;
                }
                
                var t = SelectNearUnitOf(unit, 
                    BattleUnitsModel.PlayerUnits.AppendMany(BattleUnitsModel.PlayerBuildings));
                
                if (attackModel.Target.Value != t)
                {
                    attackModel.SetTarget(t);
                }
            }
        }

        private static void UpdateDesiredPosition(BattleUnitBase unit, UnitAttackModel attackModel)
        {
            Vector3 desiredPosition;
            if (attackModel.Target.Value != null)
            {
                var direction = attackModel.Target.Value.CurrentPosition - unit.CurrentPosition;
                if (direction.sqrMagnitude < unit.GetAttackRangeSqr())
                {
                    return;
                }
                
                desiredPosition = unit.CurrentPosition - direction * (1 - 0.9f / unit.GetAttackRange());
            }
            else
            {
                desiredPosition = unit.StartPosition.Value;
            }
            unit.DesiredPosition.Set(desiredPosition);
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

        private void OnUnitDied(IBattleUnit unit)
        {
            unit.OnUnitDied -= OnUnitDied;
            
            Debug.Log($"Unit {unit.RuntimeId} died!");
        }
    }
}