using System;
using System.Collections.Generic;
using System.Linq;
using CityBuilder.Configs.Scriptable.Battle;
using CityBuilder.GameSystems.Implementation.BattleSystem.Domain;
using CityBuilder.GameSystems.Implementation.BattleSystem.Domain.Units;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;

namespace CityBuilder.GameSystems.Implementation.BattleSystem.Processing
{
    public class BattleUnitsProcessor
    {
        private readonly BattleSystemModel _battleSystemModel;
        private const float MovementThreshold = 0.05f;

        public BattleUnitsProcessor(BattleSystemModel battleSystemModel)
        {
            _battleSystemModel = battleSystemModel;
        }
        
        public void Update()
        {
            var enemyUnitsCount = _battleSystemModel.Enemies.Count;

            if (enemyUnitsCount == 0)
            {
                foreach (var playerUnit in _battleSystemModel.PlayerUnits)
                {
                    ProcessReturnToStart(playerUnit);
                }
                return;
            }

            // Process player units
            foreach (var playerUnit in _battleSystemModel.PlayerUnits)
            {
                UpdateUnit(playerUnit, true);
            }
            
            // Process enemies
            foreach (var playerUnit in _battleSystemModel.Enemies)
            {
                UpdateUnit(playerUnit, false);
            }

            // Process player buildings
            foreach (var buildingUnit in _battleSystemModel.PlayerBuildingsUnits)
            {
                UpdateUnit(buildingUnit, true);
            }
        }
        private void ProcessReturnToStart(BattleUnitBase unit)
        {
            if (unit.CanMove)
            {
                var desiredPosition = unit.StartPosition.Value;
                unit.DesiredPosition.Value = desiredPosition;
                
                TryUpdatePath(unit, true);
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
                bool force = UpdateDesiredPosition(unit, unit.AttackModel!);
                TryUpdatePath(unit, force);
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
                
                Debug.Log($"[{nameof(BattleUnitsProcessor)}] Unit {unit.Config.Name} attacks {target.Config.Name}");
                
                target.TakeDamage(damage);
                
                //TODO: On damage dealed
                if (target == _battleSystemModel.MainBuilding.Value)
                {
                    OnMainBuildingDamaged(damage, unit);
                }
            }
        }

        private void OnMainBuildingDamaged(float damage, IBattleUnit by)
        {
            Debug.LogError("Main building damaged!");
            _battleSystemModel.OnMainBuildingDamaged(damage, by);
        }

        private float GetSqrDistanceToTarget(IBattleUnit unit, UnitAttackModel attackModel)
        {
            if (attackModel.Target.Value == null)
            {
                return float.MaxValue;
            }

            return Vector3.SqrMagnitude(unit.CurrentPosition - attackModel.Target.Value.CurrentPosition);
        }

        private void TryUpdatePath(BattleUnitBase unit, bool forceUpdatePath = false)
        {
            if (unit.ThisTransform.Value == null ||
                Vector3.SqrMagnitude(unit.CurrentPosition - unit.DesiredPosition.Value) < MovementThreshold * MovementThreshold)
            {
                unit.Path.Value = null;
                return;
            }
            
            if (unit.HasPath && forceUpdatePath == false)
                return;
            
            var path = new NavMeshPath();
            NavMesh.CalculatePath(unit.CurrentPosition, unit.DesiredPosition.Value, NavMesh.AllAreas, path);
            unit.Path.Value = path;
        }

        private void ProcessMove(BattleUnitBase unit)
        {
            if (unit.ThisTransform.Value == null ||
                Vector3.SqrMagnitude(unit.CurrentPosition - unit.DesiredPosition.Value) < MovementThreshold * MovementThreshold)
            {
                return;
            }
            
            // if (unit.HasPath && forceUpdatePath == false)
            //     return;
            //
            //
            // var path = new NavMeshPath();
            // NavMesh.CalculatePath(unit.CurrentPosition, unit.DesiredPosition.Value, NavMesh.AllAreas, path);
            // path.
            
            // var deltaTime = Time.deltaTime;
            // var impulse = unit.DesiredPosition.Value - unit.CurrentPosition;
            // var delta = deltaTime * unit.GetRealMoveSpeed() * impulse.normalized;
            // unit.ThisTransform.Value.Translate(delta);
        }
        
        private void SelectTarget(IBattleUnit unit, UnitAttackModel attackModel, bool isPlayer)
        {
            // if (attackModel.HasTarget)
            // {
            //     return;
            // }
            
            IBattleUnit target;
            // Для юнитов игрока
            if (isPlayer)
            {
                target = SelectNearUnitOf(unit, _battleSystemModel.Enemies);
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
                //Debug.LogWarning(nameof(UnitsOnly));
                return SelectNearUnitOf(unit, _battleSystemModel.PlayerUnits);
            }

            IBattleUnit BuildingsOnly()
            {
                //Debug.LogWarning(nameof(BuildingsOnly));
                return SelectNearUnitOf(unit, _battleSystemModel.PlayerBuildingsUnits);
            }

            IBattleUnit DefensiveBuildingsOnly()
            {                
                //Debug.LogWarning(nameof(DefensiveBuildingsOnly));
                return SelectNearUnitOf(unit, _battleSystemModel.PlayerBuildingsUnits.Where(building => building.CanAttack));
            }

            IBattleUnit MainBuildingOnly()
            {
                //Debug.LogWarning(nameof(MainBuildingOnly));

                return _battleSystemModel.MainBuilding.Value;
            }

            IBattleUnit UnitsThenBuildings()
            {
                //Debug.LogWarning(nameof(UnitsThenBuildings));

                if (_battleSystemModel.PlayerUnits.Count > 0)
                {
                    return SelectNearUnitOf(unit, _battleSystemModel.PlayerUnits);
                }

                return SelectNearUnitOf(unit, _battleSystemModel.PlayerBuildingsUnits);
            }

            IBattleUnit UnitsThenMainBuildings()
            {
                //Debug.LogWarning(nameof(UnitsThenMainBuildings));

                if (_battleSystemModel.PlayerUnits.Count > 0)
                {
                    return SelectNearUnitOf(unit, _battleSystemModel.PlayerUnits);
                }

                return _battleSystemModel.MainBuilding.Value;
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

        private static bool UpdateDesiredPosition(BattleUnitBase unit, UnitAttackModel attackModel)
        {
            Vector3 desiredPosition;
            if (attackModel.Target.Value != null)
            {
                var direction = attackModel.Target.Value.CurrentPosition - unit.CurrentPosition;
                if (direction.sqrMagnitude < unit.GetAttackRangeSqr())
                {
                    return false;
                }
                
                desiredPosition = unit.CurrentPosition + direction * (1 - 0.9f / unit.GetAttackRange());
            }
            else
            {
                desiredPosition = unit.StartPosition.Value;
            }
            unit.DesiredPosition.Value = (desiredPosition);
            return true;
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