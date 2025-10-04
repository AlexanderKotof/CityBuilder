using System;

namespace ViewSystem
{
    public abstract class WindowViewBase<TViewModel> : ViewWithModel<TViewModel>, IWindowView
        where TViewModel : IWindowViewModel
    {
        Type IWindowView.ViewModelType => typeof(TViewModel);
    }
}