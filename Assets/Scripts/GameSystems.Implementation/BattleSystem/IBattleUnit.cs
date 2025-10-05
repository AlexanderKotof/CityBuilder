using System;
using CityBuilder.Reactive;
using Configs.Schemes.BattleSystem;
using UnityEngine;

namespace GameSystems.Implementation.BattleSystem
{
    public interface IBattleUnit : IHasHealth, IDisposable
    {
        public Guid RuntimeId { get; }
        public ReactiveProperty<Transform> ThisTransform { get; }
        
        public BattleUnitConfig Config { get; }
        
        public Vector3 CurrentPosition { get; }
        bool CanAttack { get; }
    }
    
    public interface IHasHealth
    {
        public UnitHealthAttribute Health { get; }

        void TakeDamage(float damage);
        
        event Action<IBattleUnit> OnUnitDied;
    }
}