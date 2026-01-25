using System;
using System.Collections.Generic;
using CityBuilder.Reactive;

namespace GameSystems.Implementation.BattleSystem
{
    public class BattleSystemModel
    {
        public readonly ReactiveCollection<BattleUnitBase> PlayerUnits = new();
        public readonly ReactiveCollection<BattleUnitBase> Enemies = new();
        
        public readonly ReactiveCollection<BattleUnitBase> PlayerBuildings = new();
        public readonly ReactiveProperty<BattleUnitBase> MainBuilding = new();
        
        private readonly Dictionary<Guid, Action<BattleUnitBase>> _removeUnitActions = new();
        
        public event Action<float, IBattleUnit> MainBuildingDamaged = delegate { }; 
        
        public readonly ReactiveProperty<bool> IsInBattle = new();

        public void AddPlayerUnit(BattleUnitBase unit)
        {
            if (PlayerUnits.Contains(unit) == false)
            {
                PlayerUnits.Add(unit);
                unit.OnUnitDied += OnPlayerUnitDied;
                _removeUnitActions.Add(unit.RuntimeId, RemovePlayerUnit);
            }
        }
        
        private void RemovePlayerUnit(BattleUnitBase unit)
        {
            if (PlayerUnits.Remove(unit))
            {
                unit.OnUnitDied -= OnPlayerUnitDied;
                _removeUnitActions.Remove(unit.RuntimeId);
            }
        }
        
        public void AddPlayerBuilding(BattleUnitBase unit)
        {
            if (PlayerBuildings.Contains(unit) == false)
            {
                PlayerBuildings.Add(unit);
                unit.OnUnitDied += OnPlayerBuildingUnitDied;
                _removeUnitActions.Add(unit.RuntimeId, RemovePlayerBuildingUnit);
            }
        }
        
        private void RemovePlayerBuildingUnit(BattleUnitBase unit)
        {
            if (PlayerBuildings.Remove(unit))
            {
                unit.OnUnitDied -= OnPlayerBuildingUnitDied;
                _removeUnitActions.Remove(unit.RuntimeId);
            }
        }

        public void SetMainBuilding(BattleUnitBase unit)
        {
            MainBuilding.Value = unit;
        }

        public void OnMainBuildingDamaged(float damage, IBattleUnit by) => MainBuildingDamaged?.Invoke(damage, by);

        public void AddEnemyUnit(BattleUnitBase unit)
        {
            if (Enemies.Contains(unit) == false)
            {
                Enemies.Add(unit);
                unit.OnUnitDied += OnEnemyUnitDied;
                _removeUnitActions.Add(unit.RuntimeId, RemoveEnemyUnit);
            }
        }
        
        public void RemoveEnemyUnit(BattleUnitBase unit)
        {
            if (Enemies.Remove(unit))
            {
                unit.OnUnitDied -= OnEnemyUnitDied;
                _removeUnitActions.Remove(unit.RuntimeId);
            }
        }
        
        public void RemoveUnit(BattleUnitBase unit)
        {
            if (_removeUnitActions.TryGetValue(unit.RuntimeId, out Action<BattleUnitBase> action))
            {
                action.Invoke(unit);
            }
        }
        
        private void OnPlayerUnitDied(IBattleUnit died) => RemovePlayerUnit(died as BattleUnitBase);
        private void OnPlayerBuildingUnitDied(IBattleUnit died) => RemovePlayerUnit(died as BattleUnitBase);
        private void OnEnemyUnitDied(IBattleUnit unit) => RemoveEnemyUnit(unit as BattleUnitBase);

    }
}