using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameSystems.Common.ViewSystem.ViewsProvider;
using GameSystems.Implementation.BuildingSystem.Domain;
using JetBrains.Annotations;
using UniRx;
using UnityEngine;
using Views.Implementation.BuildingSystem;

namespace GameSystems.Common.ViewSystem
{
    public class ViewsCollectionController<TView> : IDisposable where TView : Component
    {
        private readonly IViewsProvider _viewsProvider;
        [CanBeNull]
        private readonly string _defaultAssetKey;
        [CanBeNull]
        private readonly Transform _defaultParent;
        
        private readonly Dictionary<object, TView> _activeViews = new();

        public ViewsCollectionController(IViewsProvider viewsProvider, string defaultAssetKey = null, Transform defaultParent = null)
        {
            _viewsProvider = viewsProvider;
            _defaultAssetKey = defaultAssetKey;
            _defaultParent = defaultParent;
        }

        public void Dispose()
        {
            foreach (var view in _activeViews.Values)
            {
                Recycle(view);
            }
            _activeViews.Clear();
        }
        
        public async UniTask<TView> AddView(string assetKey, object data, Transform parent)
        {
            var view = await _viewsProvider.ProvideViewAsync<TView>(assetKey, parent);
            _activeViews.Add(data, view);
            return view;
        }
        
        public UniTask<TView> AddView(string assetKey, object data)
        {
            return AddView(assetKey, data, _defaultParent);
        }
        
        public UniTask<TView> AddView(object data)
        {
            return AddView(_defaultAssetKey, data, _defaultParent);
        }
        
        public UniTask<TView> AddView(object data, Transform parent)
        {
            return AddView(_defaultAssetKey, data, parent);
        }
        
        private void Recycle(TView view)
        {
            _viewsProvider.ReturnView(view);
        }

        public void Return(object data)
        {
            if (_activeViews.Remove(data, out var view))
            {
                Recycle(view);
            }
        }

        public bool TryGetView(object data, out TView view)
        {
            return _activeViews.TryGetValue(data, out view);
        }

        public TView GetView(object data)
        {
            return _activeViews[data];
        }
    }
}