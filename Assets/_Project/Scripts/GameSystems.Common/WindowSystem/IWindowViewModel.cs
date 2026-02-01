using System;
using CityBuilder.GameSystems.Common.ViewSystem;
using UniRx;

namespace CityBuilder.GameSystems.Common.WindowSystem
{
    public interface IWindowViewModel : IViewModel, IDisposable
    {
        ReactiveProperty<bool> IsActive { get; }

        ReactiveCommand Close { get; }
    }
}