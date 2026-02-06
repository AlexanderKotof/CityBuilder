using System;
using CityBuilder.Configs.Scriptable.Battle;
using CityBuilder.GameSystems.Common.ViewSystem;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace CityBuilder.GameSystems.Implementation.BattleSystem.Domain.Units
{
    public class BattleUnitBase : IViewModel, IBattleUnit
    {
        public Guid RuntimeId { get; } = Guid.NewGuid();
        
        public BattleUnitConfigSO Config { get; private set; }
        
        public UnitHealthAttribute Health { get; }
        public bool IsAlive => Health.CurrentValue > 0;
        public event Action<IBattleUnit>? OnUnitDied;

        public IObservable<BattleUnitBase> OnDiedObservable => _onDie;
        private readonly Subject<BattleUnitBase> _onDie = new();
        
        public ReactiveProperty<NavMeshPath> Path { get; } = new();
        public bool HasPath => Path.Value != null && Path.Value.corners.Length > 0;
        
        public ReactiveProperty<Transform> ThisTransform { get; } = new();
        public Vector3 CurrentPosition => ThisTransform.Value?.position + _centerOffset ?? Vector3.zero;
        
        // Position as observable?
        // public IObservable<Vector3> PositionObservable => ThisTransform.Value.O

        public UnitAttackModel? AttackModel { get; }
        
        public bool CanAttack => Config.Damage > 0 && Config.AttackSpeed > 0 && Config.AttackRange > 0;
        public bool CanMove => Config.MoveSpeed > 0;
        
        private readonly Vector3 _centerOffset = Vector3.zero;
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
        
        public BattleUnitBase(BattleUnitConfigSO config, int level, Vector3 startPosition, ReactiveProperty<Transform> transform, Vector3 centerOffset) : this(config, level, startPosition)
        {
            ThisTransform = transform;
            _centerOffset = centerOffset;
        }
        
        public BattleUnitBase(BattleUnitConfigSO config, int level, Vector3 startPosition) : this(config)
        {
            StartPosition.Value = (startPosition);
        }

        public void TakeDamage(float damage)
        {
            if (IsAlive == false)
                return;
            
            Health.CurrentValue.Value -= Mathf.Min(damage, Health.CurrentValue.Value);
            
            Debug.Log($"[{nameof(BattleUnitBase)}] Unit {Config.Name} Received {damage.ToString()} damage");

            if (IsAlive == false)
            {
                _onDie.OnNext(this);
                OnUnitDied?.Invoke(this);
            }
        }

        public virtual void Dispose()
        {
            OnUnitDied = null;
            _onDie.Dispose();
            ThisTransform.Dispose();
        }

        public void ApplyConfig(BattleUnitConfigSO config)
        {
            Config = config;
            Health.CurrentValue.Value = config.Health;
            Health.StartValue.Value = config.Health;
        }
    }
}