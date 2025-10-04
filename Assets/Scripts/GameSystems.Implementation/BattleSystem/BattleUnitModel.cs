using System;
using CityBuilder.Reactive;
using Configs.Schemes.BattleSystem;
using UnityEngine;
using ViewSystem;

namespace GameSystems.Implementation.BattleSystem
{
    public class BattleUnitModel : IViewModel, IBattleUnit
    {
        public Guid Id { get; } = Guid.NewGuid();
        
        public ReactiveProperty<Vector3> StartPosition { get; } = new();
        public ReactiveProperty<Vector3> DesiredPosition { get; } = new();
        public ReactiveProperty<Vector3> CurrentPosition { get; } = new();
        
        public ReactiveProperty<BattleUnitModel?> Target { get; } = new();
        
        public ReactiveProperty<float> LastAttackTime { get; } = new();
        
        
        public UnitHealthAttribute Health { get; } = new();
        public BattleUnitConfig Config { get; }
        
        
        public ReactiveProperty<Transform?> ThisTransform { get; } = new();
        
        public event Action<BattleUnitModel> OnUnitDied;

        public BattleUnitModel(BattleUnitConfig config, int level, Vector3 startPosition)
        {
            Config = config;
            StartPosition.Set(startPosition);
            CurrentPosition.Value = startPosition;
        }

        public void TakeDamage(float damage)
        {
            //TODO: calculate damage by defence and etc
            
            Health.CurrentValue.Value -= Mathf.Min(damage, Health.CurrentValue.Value);
            
            Debug.Log("Taking damage {}");

            if (Health.CurrentValue.Value <= 0)
            {
                //TODO: unit is died
                OnUnitDied?.Invoke(this);
            }
        }
    }
}