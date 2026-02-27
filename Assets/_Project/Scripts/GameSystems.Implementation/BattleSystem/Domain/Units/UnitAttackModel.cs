using CityBuilder.GameSystems.Common.ViewSystem;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace CityBuilder.GameSystems.Implementation.BattleSystem.Domain.Units
{
    public class UnitAttackModel : IViewModel
    {
        public readonly ReactiveProperty<Transform?> MainTargetTransform = new();
        
        public readonly ReactiveProperty<IBattleUnit?> MainTarget = new();
        
        //Used for multi targets attackers, not all of enemies on the map
        public readonly ReactiveCollection<IBattleUnit> AllTargets = new();

        public ReactiveProperty<float> LastAttackTime { get; } = new();
        public bool HasMainTarget => MainTarget.Value is { IsAlive: true };

        public void SetMainTarget([CanBeNull] IBattleUnit unit)
        {
            if (unit == null)
            {
                MainTarget.Value = null;
                MainTargetTransform.Value = null;
                return;
            }

            MainTarget.Value = unit;
            MainTargetTransform.Value = unit.ThisTransform.Value;
        }

        public void AddTarget(IBattleUnit unit)
        {
            if (unit == null || AllTargets.Contains(unit))
                return;
            
            AllTargets.Add(unit);
        }
        
        public void RemoveTarget(IBattleUnit unit)
        {
            if (unit == null)
                return;
            
            AllTargets.Remove(unit);
        }
    }
}