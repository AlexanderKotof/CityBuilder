using System;
using System.Collections.Generic;
using System.Threading;
using CityBuilder.GameSystems.Common.ViewSystem.ViewsProvider;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace CityBuilder.GameSystems.Common.ViewSystem
{
    public class ViewsCollectionController<TView> : IDisposable where TView : Component
    {
        private readonly IViewsProvider _viewsProvider;
        [CanBeNull]
        private readonly string _defaultAssetKey;
        [CanBeNull]
        private readonly Transform _defaultParent;
        
        private readonly Dictionary<object, TView> _activeViews = new();
        private readonly Dictionary<object, CancellationTokenSource> _cancellationTokenSources = new();

        public ViewsCollectionController(IViewsProvider viewsProvider, string defaultAssetKey = null, Transform defaultParent = null)
        {
            _viewsProvider = viewsProvider;
            _defaultAssetKey = defaultAssetKey;
            _defaultParent = defaultParent;
        }

        public void Dispose()
        {
            foreach (var cts in _cancellationTokenSources.Values)
            {
                cts.Cancel();
            }
            _cancellationTokenSources.Clear();
            
            foreach (var view in _activeViews.Values)
            {
                Recycle(view);
            }
            _activeViews.Clear();
        }
        
        public async UniTask<TView> AddView(string assetKey, object data, Transform parent)
        {
            var cts = new CancellationTokenSource();
            try
            {
                _cancellationTokenSources.Add(data, cts);
                var view = await _viewsProvider
                    .ProvideViewAsync<TView>(assetKey, parent)
                    .AttachExternalCancellation(cts.Token);
                _activeViews.Add(data, view);
                return view;
            }
            finally
            {
                cts.Dispose();
                _cancellationTokenSources.Remove(data);
            }
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
            if (_cancellationTokenSources.TryGetValue(data, out var cts))
            {
                cts.Cancel();
            }
            
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