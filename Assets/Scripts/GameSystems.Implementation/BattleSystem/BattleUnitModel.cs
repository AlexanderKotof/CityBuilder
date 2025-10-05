using System;
using CityBuilder.BuildingSystem;
using CityBuilder.Reactive;
using Configs.Schemes.BattleSystem;
using JetBrains.Annotations;
using UnityEngine;
using ViewSystem;

namespace GameSystems.Implementation.BattleSystem
{
    public class UnitAttackModel : IViewModel
    {
        public readonly ReactiveProperty<Transform?> TargetTransform = new();
        
        public readonly ReactiveProperty<IBattleUnit?> Target = new();
        
        public ReactiveProperty<float> LastAttackTime { get; } = new();

        public void SetTarget([CanBeNull] IBattleUnit unit)
        {
            if (unit == null)
            {
                Target.Value = null;
                TargetTransform.Value = null;
                return;
            }

            Target.Value = unit;
            TargetTransform.Value = unit.ThisTransform.Value;
        }
        
    }

    public class BattleUnitBase : IViewModel, IBattleUnit
    {
        public Guid RuntimeId { get; } = Guid.NewGuid();
        
        public BattleUnitConfig Config { get; }
        
        public UnitHealthAttribute Health { get; }
        public event Action<IBattleUnit>? OnUnitDied;
        
        public ReactiveProperty<Transform?> ThisTransform { get; } = new();
        public Vector3 CurrentPosition => ThisTransform.Value?.position ?? Vector3.zero;

        public UnitAttackModel? AttackModel { get; }
        
        public bool CanAttack => Config.Damage > 0 && Config.AttackSpeed > 0 && Config.AttackRange > 0;
        public bool CanMove => Config.MoveSpeed > 0;
        
        public BattleUnitBase(BattleUnitConfig config)
        {
            Config = config;
            Health = new UnitHealthAttribute(config.Health);

            if (CanAttack)
            {
                AttackModel = new();
            }
        }
        
        public void TakeDamage(float damage)
        {
            Health.CurrentValue.Value -= Mathf.Min(damage, Health.CurrentValue.Value);
            
            Debug.Log($"Taking damage {damage.ToString()}");

            if (Health.CurrentValue.Value <= 0)
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

    public class BuildingBattleUnitModel : BattleUnitBase
    {
        private readonly BuildingModel _building;

        public BuildingBattleUnitModel(BattleUnitConfig config, BuildingModel building) : base(config)
        {
            _building = building;
        }
    }
    
    public class BattleUnitModel : BattleUnitBase
    {
        public ReactiveProperty<Vector3> StartPosition { get; } = new();
        public ReactiveProperty<Vector3> DesiredPosition { get; } = new();

        public BattleUnitModel(BattleUnitConfig config, int level, Vector3 startPosition) : base(config)
        {
            StartPosition.Set(startPosition);
        }
    }
}