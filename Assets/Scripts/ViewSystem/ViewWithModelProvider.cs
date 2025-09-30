using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CityBuilder.Dependencies;
using UnityEngine;

namespace ViewSystem
{
    public interface IViewWithModelProvider
    {
        public Task<ViewWithModel<TViewModel>> ProvideViewWithModel<TViewModel>(
            string assetKey,
            TViewModel viewModel,
            Transform parent = null) where TViewModel : IViewModel;

        public Task<TView> ProvideViewWithModel<TViewModel, TView>(
            string assetKey,
            TViewModel viewModel,
            Transform parent = null)
            where TViewModel : IViewModel
            where TView : ViewWithModel<TViewModel>;

        public void Recycle<TViewModel>(TViewModel viewModel)
            where TViewModel : IViewModel;
    }

    public class ViewWithModelProvider : IViewWithModelProvider
    {
        private record Window(IViewModel ViewModel, ViewBase View, Action DeinitAction) : IDisposable
        {
            public IViewModel ViewModel { get; } = ViewModel;
            public ViewBase View { get; } = View;

            public Action DeinitAction { get; } = DeinitAction;

            public void Dispose()
            {
                
            }
        }
        
        private readonly Dictionary<IViewModel, Window> _viewModelViews = new();

        private readonly IViewsProvider _viewsProvider;
        private readonly IDependencyContainer _dependencies;

        public ViewWithModelProvider(IViewsProvider viewsProvider, IDependencyContainer dependencies)
        {
            _viewsProvider = viewsProvider;
            _dependencies = dependencies;
        }

        public void Deinit()
        {
            foreach (var window in _viewModelViews.Values)
            {
                DeinitWindow(window);
            }
            _viewModelViews.Clear();
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

            viewGo.Initialize(viewModel, _dependencies);
            
            var window = new Window(viewModel, viewGo, viewGo.Deinit);
            _viewModelViews.Add(viewModel, window);
            
            return viewGo;
        }

        public void Recycle<TViewModel>(TViewModel viewModel)
            where TViewModel : IViewModel
        {
            if (_viewModelViews.TryGetValue(viewModel, out var window))
            {
                DeinitWindow(window);
                _viewModelViews.Remove(viewModel);
            }
        }

        private void DeinitWindow(Window window)
        {
            window.DeinitAction();
            _viewsProvider.ReturnView(window.View);
            window.Dispose();
        }
    }
}