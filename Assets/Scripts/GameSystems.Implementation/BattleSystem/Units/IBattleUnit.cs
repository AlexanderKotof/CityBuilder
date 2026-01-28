using System;
using CityBuilder.Reactive;
using Configs.Schemes.BattleSystem;
using Configs.Scriptable;
using UnityEngine;

namespace GameSystems.Implementation.BattleSystem
{
    public interface IBattleUnit : IHasHealth, IDisposable
    {
        public Guid RuntimeId { get; }
        public ReactiveProperty<Transform> ThisTransform { get; }
        
        public BattleUnitConfigSO Config { get; }
        
        public Vector3 CurrentPosition { get; }
        
        public UnitAttackModel? AttackModel { get; }
        
        bool CanAttack { get; }
    }
}