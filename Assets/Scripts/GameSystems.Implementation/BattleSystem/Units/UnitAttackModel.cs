using CityBuilder.Reactive;
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
        public bool HasTarget => Target.Value is { IsAlive: true };

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
}