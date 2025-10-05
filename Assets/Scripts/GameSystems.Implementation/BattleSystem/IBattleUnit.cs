using System;
using CityBuilder.Reactive;
using UnityEngine;

namespace GameSystems.Implementation.BattleSystem
{
    public interface IBattleUnit : IHasHealth, IDisposable
    {
        public Guid RuntimeId { get; }
        public ReactiveProperty<Transform> ThisTransform { get; }
        
        public Vector3 CurrentPosition { get; }
    }
    
    public interface IHasHealth
    {
        public UnitHealthAttribute Health { get; }

        void TakeDamage(float damage);
        
        event Action<IBattleUnit> OnUnitDied;
    }
}