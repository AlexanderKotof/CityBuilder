using System;
using CityBuilder.Reactive;
using Configs.Scriptable;
using Configs.Scriptable.Battle;
using GameSystems.Common.ViewSystem;
using UnityEngine;
using ViewSystem;

namespace GameSystems.Implementation.BattleSystem
{
    public class BattleUnitBase : IViewModel, IBattleUnit
    {
        public Guid RuntimeId { get; } = Guid.NewGuid();
        
        public BattleUnitConfigSO Config { get; }
        
        public UnitHealthAttribute Health { get; }
        public bool IsAlive => Health.CurrentValue > 0;
        public event Action<IBattleUnit>? OnUnitDied;
        
        public ReactiveProperty<Transform?> ThisTransform { get; } = new();
        public Vector3 CurrentPosition => ThisTransform.Value?.position ?? Vector3.zero;

        public UnitAttackModel? AttackModel { get; }
        
        public bool CanAttack => Config.Damage > 0 && Config.AttackSpeed > 0 && Config.AttackRange > 0;
        public bool CanMove => Config.MoveSpeed > 0;
        
        public ReactiveProperty<Vector3> StartPosition { get; } = new();
        public ReactiveProperty<Vector3> DesiredPosition { get; } = new();
        
        private BattleUnitBase(BattleUnitConfigSO config)
        {
            Config = config;
            Health = new UnitHealthAttribute(config.Health);

            if (CanAttack)
            {
                AttackModel = new();
            }
        }
        
        public BattleUnitBase(BattleUnitConfigSO config, int level, Vector3 startPosition, Transform transform) : this(config, level, startPosition)
        {
            ThisTransform.Set(transform);
        }

        public BattleUnitBase(BattleUnitConfigSO config, int level, Vector3 startPosition) : this(config)
        {
            StartPosition.Set(startPosition);
        }

        public void TakeDamage(float damage)
        {
            if (IsAlive == false)
                return;
            
            Health.CurrentValue.Value -= Mathf.Min(damage, Health.CurrentValue.Value);
            
            Debug.Log($"[{nameof(BattleUnitBase)}] Unit {Config.Name} Received {damage.ToString()} damage");

            if (IsAlive == false)
            {
                //TODO: unit is died
                OnUnitDied?.Invoke(this);
            }
        }

        public virtual void Dispose()
        {
            OnUnitDied = null;
            ThisTransform.Dispose();
        }
    }
}