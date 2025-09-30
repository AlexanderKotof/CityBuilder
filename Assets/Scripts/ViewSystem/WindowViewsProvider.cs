using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace ViewSystem
{
    public class WindowViewsProvider
    {
        private record Window(IViewModel ViewModel, ViewBase View, Action DeinitAction)
        {
            public IViewModel ViewModel { get; } = ViewModel;
            public ViewBase View { get; } = View;

            public Action DeinitAction { get; } = DeinitAction;
        }
        
        private readonly Dictionary<IViewModel, Window> _viewModelViews = new();

        private readonly ViewsProvider _viewsProvider;

        public WindowViewsProvider(ViewsProvider viewsProvider)
        {
            _viewsProvider = viewsProvider;
        }
        
        public Task<ViewWithModel<TViewModel>> ProvideViewWithModel<TViewModel>(string assetKey, TViewModel viewModel,
            Transform parent = null) where TViewModel : IViewModel
        {
            return ProvideViewWithModel<TViewModel, ViewWithModel<TViewModel>>(assetKey, viewModel, parent);
        }
        
        public async Task<TView> ProvideViewWithModel<TViewModel, TView>(string assetKey, TViewModel viewModel,
            Transform parent = null)
            where TViewModel : IViewModel
            where TView : ViewWithModel<TViewModel>
        {
            var viewGo = await _viewsProvider.ProvideViewAsync<TView>(assetKey, parent);

            viewGo.Initialize(viewModel);
            
            var window = new Window(viewModel, viewGo, viewGo.Deinit);
            _viewModelViews.Add(viewModel, window);

            return viewGo;
        }

        public void Recycle<TViewModel>(TViewModel viewModel)
            where TViewModel : IViewModel
        {
            if (_viewModelViews.TryGetValue(viewModel, out var window))
            {
                window.DeinitAction.Invoke();
                
                _viewsProvider.ReturnView(window.View);
                
                _viewModelViews.Remove(viewModel);
            }
        }
        
        // public void ReturnView<TViewModel>(ViewWithModel<TViewModel> view) where TViewModel : IViewModel
        // {
        //     view.Deinit();
        //     ReturnView(view.gameObject);
        // }
    }
}