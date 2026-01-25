using System;

namespace GameSystems.Implementation.BattleSystem
{
    public interface IHasHealth
    {
        public UnitHealthAttribute Health { get; }
        
        bool IsAlive { get; }

        void TakeDamage(float damage);
        
        event Action<IBattleUnit> OnUnitDied;
    }
}