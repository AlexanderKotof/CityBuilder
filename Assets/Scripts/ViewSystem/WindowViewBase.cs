using System;

namespace ViewSystem
{
    public abstract class WindowViewBase<TViewModel> : ViewWithModel<TViewModel>, IWindow
        where TViewModel : IViewModel
    {
        //public abstract string AssetId { get; }

        Type IWindow.ViewModelType => typeof(TViewModel);
    }
}