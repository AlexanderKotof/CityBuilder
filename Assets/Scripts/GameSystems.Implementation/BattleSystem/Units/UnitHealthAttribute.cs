using CityBuilder.Reactive;
using ViewSystem;

namespace GameSystems.Implementation.BattleSystem
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