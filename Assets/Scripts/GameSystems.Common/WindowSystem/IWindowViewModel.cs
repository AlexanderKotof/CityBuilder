using System;
using GameSystems.Common.ViewSystem;
using UniRx;

namespace GameSystems.Common.WindowSystem
{
    public interface IWindowViewModel : IViewModel, IDisposable
    {
        ReactiveProperty<bool> IsActive { get; }

        ReactiveCommand Close { get; }
    }
}