using System.Collections.Generic;
using CityBuilder.Dependencies;
using CityBuilder.GameSystems.Common.ViewSystem.View;
using CityBuilder.GameSystems.Common.ViewSystem.ViewsProvider;
using CityBuilder.Reactive;
using JetBrains.Annotations;
using UnityEngine;

namespace CityBuilder.GameSystems.Common.ViewSystem
{
    public abstract class ReactiveCollectionViewBase<TViewModel, TView> 
        where TViewModel : IViewModel
        where TView : ViewWithModel<TViewModel>
    {
        [CanBeNull] 
        private readonly Transform _parent;

        private readonly IViewWithModelProvider _viewsProvider;
        private readonly ReactiveCollection<TViewModel> _collection;
        
        private readonly Dictionary<TViewModel, TView> _views = new();
        private readonly IDependencyContainer _dependencies;

        protected ReactiveCollectionViewBase(
            ReactiveCollection<TViewModel> collection,
            IDependencyContainer dependencies,
            Transform parent = null)
        {
            _parent = parent;
            _collection = collection;
            _dependencies = dependencies;
            _viewsProvider = dependencies.Resolve<IViewWithModelProvider>();
        }

        public void Initialize()
        {
            _collection.SubscribeAdd(OnViewModelAdded);
            _collection.SubscribeRemove(OnViewModelRemoved);

            foreach (var viewModel in _collection)
            {
                OnViewModelAdded(viewModel);
            }
        }
        
        public void Deinit()
        {
            _collection.UnsubscribeAdd(OnViewModelAdded);
            _collection.UnsubscribeRemove(OnViewModelRemoved);
        }

        private async void OnViewModelAdded(TViewModel viewModel)
        {
            var view = await _viewsProvider.ProvideViewWithModel<TViewModel, TView>(
                ProvideAssetKey(viewModel),
                viewModel,
                _dependencies,
                _parent);
            
            _views.Add(viewModel, view);

            OnViewAdded(viewModel, view);
        }
        
        private void OnViewModelRemoved(TViewModel viewModel)
        {
            if (_views.Remove(viewModel, out var view))
            {
                _viewsProvider.Recycle(viewModel);
                OnViewRemoved(viewModel, view);
            }
        }
        
        protected virtual void OnViewAdded(TViewModel viewModel, TView view) { }
        protected virtual void OnViewRemoved(TViewModel viewModel, TView value) { }

        protected abstract string ProvideAssetKey(TViewModel viewModel);
        
        public bool TryGetView(TViewModel viewModel, out TView view)
            => _views.TryGetValue(viewModel, out view);
    }
}