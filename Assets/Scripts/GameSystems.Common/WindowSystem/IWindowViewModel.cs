using CityBuilder.Reactive;
using ViewSystem;

public interface IWindowViewModel : IViewModel
{
    ReactiveProperty<bool> IsActive { get; }

    ReactiveCommand Close { get; }
}