using System;

namespace CityBuilder.GameSystems.Implementation.BattleSystem.Domain.Units
{
    public interface IHasHealth
    {
        public UnitHealthAttribute Health { get; }
        
        bool IsAlive { get; }

        void TakeDamage(float damage);
        
        event Action<IBattleUnit> OnUnitDied;
    }
}