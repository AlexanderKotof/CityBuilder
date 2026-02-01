using CityBuilder.GameSystems.Common.ViewSystem;
using CityBuilder.Reactive;

namespace CityBuilder.GameSystems.Implementation.BattleSystem.Domain.Units
{
    public class UnitHealthAttribute : IViewModel
    {
        public readonly ReactiveProperty<float> CurrentValue = new ();
        public readonly ReactiveProperty<float> StartValue = new ();

        public bool IsFull => CurrentValue.Value >= StartValue.Value;
        
        public UnitHealthAttribute(float startValue)
        {
            CurrentValue.Set(startValue);
            StartValue.Set(startValue);
        }

    }
}