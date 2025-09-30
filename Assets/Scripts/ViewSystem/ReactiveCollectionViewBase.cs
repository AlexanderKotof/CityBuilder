using System.Collections.Generic;
using JetBrains.Annotations;
using CityBuilder.Reactive;
using UnityEngine;

namespace ViewSystem
{
    public abstract class ReactiveCollectionViewBase<TViewModel, TView> 
        where TViewModel : IViewModel
        where TView : ViewWithModel<TViewModel>
    {
        [CanBeNull] 
        private readonly Transform _parent;

        private readonly WindowViewsProvider _viewsProvider;
        public ReactiveCollection<TViewModel> Collection { get; }
        
        private readonly Dictionary<TViewModel, TView> _views = new Dictionary<TViewModel, TView>();
        
        public ReactiveCollectionViewBase(
            ReactiveCollection<TViewModel> collection,
            WindowViewsProvider viewsProvider,
            Transform parent = null)
        {
            _parent = parent;
            _viewsProvider = viewsProvider;
            
            Collection = collection;
        }

        public void Initialize()
        {
            Collection.SubscribeAdd(OnViewModelAdded);
            Collection.SubscribeRemove(OnViewModelRemoved);

            foreach (var viewModel in Collection)
            {
                OnViewModelAdded(viewModel);
            }
        }
        
        public void Deinit()
        {
            Collection.UnsubscribeAdd(OnViewModelAdded);
            Collection.UnsubscribeRemove(OnViewModelRemoved);
        }

        private async void OnViewModelAdded(TViewModel viewModel)
        {
            var view = await _viewsProvider.ProvideViewWithModel<TViewModel, TView>(
                ProvideAssetKey(viewModel),
                viewModel,
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