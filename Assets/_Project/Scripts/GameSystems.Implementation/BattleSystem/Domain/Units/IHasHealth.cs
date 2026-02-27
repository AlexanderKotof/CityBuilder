using System;

namespace CityBuilder.GameSystems.Implementation.BattleSystem.Domain.Units
{
    public interface IHasHealth
    {
        public UnitHealthAttribute Health { get; }
        
        bool IsAlive { get; }

        void TakeDamage(float damage);

        IObservable<float> OnDamaged { get; }
        
        event Action<IBattleUnit> OnUnitDied;
    }
}