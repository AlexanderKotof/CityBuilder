using System;
using CityBuilder.GameSystems.Common.ViewSystem.View;

namespace CityBuilder.GameSystems.Common.WindowSystem.Window
{
    public abstract class WindowViewBase<TViewModel> : ViewWithModel<TViewModel>, IWindowView
        where TViewModel : IWindowViewModel
    {
        Type IWindowView.ViewModelType => typeof(TViewModel);
    }
}