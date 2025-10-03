using CityBuilder.Reactive;

namespace GameSystems.Implementation.BattleSystem
{
    public class BattleUnitsModel
    {
        public readonly ReactiveCollection<BattleUnitModel> PlayerUnits = new();
        public readonly ReactiveCollection<BattleUnitModel> Enemies = new();
        
        public bool IsInBattleState => Enemies.Count > 0;

        public void AddPlayerUnit(BattleUnitModel unit)
        {
            if (PlayerUnits.Contains(unit) == false)
            {
                PlayerUnits.Add(unit);
                unit.OnUnitDied += OnPlayerUnitDied;
            }
        }
        
        public void RemovePlayerUnit(BattleUnitModel unit)
        {
            if (PlayerUnits.Remove(unit))
            {
                unit.OnUnitDied -= OnPlayerUnitDied;
            }
        }
        
        public void AddEnemyUnit(BattleUnitModel unit)
        {
            if (Enemies.Contains(unit) == false)
            {
                Enemies.Add(unit);
                unit.OnUnitDied += OnEnemyUnitDied;
            }
        }
        
        public void RemoveEnemyUnit(BattleUnitModel unit)
        {
            if (Enemies.Remove(unit))
            {
                unit.OnUnitDied -= OnEnemyUnitDied;
            }
        }
        
        private void OnPlayerUnitDied(BattleUnitModel died) => RemovePlayerUnit(died);
        private void OnEnemyUnitDied(BattleUnitModel unit) => RemoveEnemyUnit(unit);
    }
}