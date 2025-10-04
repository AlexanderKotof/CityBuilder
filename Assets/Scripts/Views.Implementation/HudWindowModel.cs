using CityBuilder.Reactive;

namespace ViewSystem
{
    public class HudWindowModel : IWindowViewModel
    {
        public ReactiveProperty<bool> IsActive { get; } = new();
        public ReactiveCommand Close { get; } = new();
    }
}