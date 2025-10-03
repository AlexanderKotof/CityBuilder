using CityBuilder.Reactive;
using ViewSystem;

namespace GameSystems.Implementation.BattleSystem
{
    public class UnitHealthAttribute : IViewModel
    {
        public readonly ReactiveProperty<float> CurrentValue = new ();
        public readonly ReactiveProperty<float> StartValue = new ();
    }
}