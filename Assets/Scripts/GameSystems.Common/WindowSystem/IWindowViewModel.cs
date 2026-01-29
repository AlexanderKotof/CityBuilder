using CityBuilder.Reactive;
using GameSystems.Common.ViewSystem;
using ViewSystem;

public interface IWindowViewModel : IViewModel
{
    ReactiveProperty<bool> IsActive { get; }

    ReactiveCommand Close { get; }
}