using System;
using System.Collections.Generic;
using System.Linq;
using Configs.Schemes.BattleSystem;
using JetBrains.Annotations;
using UnityEngine;

namespace GameSystems.Implementation.BattleSystem
{
    public class BattleUnitsProcessor
    {
        private readonly BattleUnitsModel _battleUnitsModel;

        public BattleUnitsProcessor(BattleUnitsModel battleUnitsModel)
        {
            _battleUnitsModel = battleUnitsModel;
        }
        
        public void Update()
        {
            var enemyUnitsCount = _battleUnitsModel.Enemies.Count;

            if (enemyUnitsCount == 0)
            {
                foreach (var playerUnit in _battleUnitsModel.PlayerUnits)
                {
                    ProcessReturnToStart(playerUnit);
                }
                return;
            }

            // Process player units
            foreach (var playerUnit in _battleUnitsModel.PlayerUnits)
            {
                UpdateUnit(playerUnit, true);
            }
            
            // Process enemies
            foreach (var playerUnit in _battleUnitsModel.Enemies)
            {
                UpdateUnit(playerUnit, false);
            }

            // Process player buildings
            foreach (var buildingUnit in _battleUnitsModel.PlayerBuildings)
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
            if (attackModel.HasTarget)
            {
                return;
            }
            
            IBattleUnit target;
            // Для юнитов игрока
            if (isPlayer)
            {
                target = SelectNearUnitOf(unit, _battleUnitsModel.Enemies);
                attackModel.SetTarget(target);
                return;
            }

            // Для энемисов используем стратегию на основе конфига
            var strategy = GetTargetSelectionStrategy(unit);
            target = strategy();
            attackModel.SetTarget(target);
        }

        private Func<IBattleUnit> GetTargetSelectionStrategy(IBattleUnit unit)
        {
            IBattleUnit UnitsOnly()
            {
                Debug.LogWarning(nameof(UnitsOnly));
                return SelectNearUnitOf(unit, _battleUnitsModel.PlayerUnits);
            }

            IBattleUnit BuildingsOnly()
            {
                Debug.LogWarning(nameof(BuildingsOnly));
                return SelectNearUnitOf(unit, _battleUnitsModel.PlayerBuildings);
            }

            IBattleUnit DefensiveBuildingsOnly()
            {                
                Debug.LogWarning(nameof(DefensiveBuildingsOnly));
                return SelectNearUnitOf(unit, _battleUnitsModel.PlayerBuildings.Where(building => building.CanAttack));
            }

            IBattleUnit MainBuildingOnly()
            {
                Debug.LogWarning(nameof(MainBuildingOnly));

                return _battleUnitsModel.MainBuilding.Value;
            }

            IBattleUnit UnitsThenBuildings()
            {
                Debug.LogWarning(nameof(UnitsThenBuildings));

                if (_battleUnitsModel.PlayerUnits.Count > 0)
                {
                    return SelectNearUnitOf(unit, _battleUnitsModel.PlayerUnits);
                }

                return SelectNearUnitOf(unit, _battleUnitsModel.PlayerBuildings);
            }

            IBattleUnit UnitsThenMainBuildings()
            {
                Debug.LogWarning(nameof(UnitsThenMainBuildings));

                if (_battleUnitsModel.PlayerUnits.Count > 0)
                {
                    return SelectNearUnitOf(unit, _battleUnitsModel.PlayerUnits);
                }

                return _battleUnitsModel.MainBuilding.Value;
            }

            return unit.Config.AttackPossibilityAndPriority switch
            {
                AttackPossibilityAndPriority.UnitsOnly => UnitsOnly,
                AttackPossibilityAndPriority.BuildingsOnly => BuildingsOnly,
                AttackPossibilityAndPriority.DefensiveBuildingsOnly => DefensiveBuildingsOnly,
                AttackPossibilityAndPriority.MainBuildingOnly => MainBuildingOnly,
                AttackPossibilityAndPriority.UnitsThenBuildings => UnitsThenBuildings,
                AttackPossibilityAndPriority.UnitsThenMainBuildings => UnitsThenMainBuildings,
                _ => throw new NotImplementedException(),
            };
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
                
                desiredPosition = unit.CurrentPosition + direction * (1 - 0.9f / unit.GetAttackRange());
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

    }
}