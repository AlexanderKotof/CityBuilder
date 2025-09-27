using System.Collections.Generic;
using CityBuilder.BuildingSystem;
using UnityEngine;

namespace ViewSystem
{
    public class ViewsProvider
    {
        private readonly Dictionary<string, ViewPool> _objectPools = new();

        private readonly Dictionary<string, GameObjectPool> _gameObjectPools = new();
        
        public GameObject ProvideView(GameObject prefab, Transform parent = null)
        {
            string assetKey = GetAssetKey(prefab);
            if (_gameObjectPools.TryGetValue(assetKey, out var pool))
            {
                return pool.Pool(parent);
            }

            pool = new GameObjectPool(prefab);
            _gameObjectPools.Add(assetKey, pool);

            return pool.Pool(parent);
        }

        public void ReturnView(GameObject gameObject)
        {
            if (_gameObjectPools.TryGetValue(GetAssetKey(gameObject), out var pool))
            {
                pool.Return(gameObject);
            }
        }

        private string GetAssetKey(GameObject gameObject)
        {
            return gameObject.name;
        }
/*
        public T ProvideView<T>(T prefab, Transform parent = null) where T : ViewBase
        {
            if (_objectPools.TryGetValue(prefab.AssetId, out var pool))
            {
                return pool.Pool(parent) as T;
            }

            pool = new ViewPool(prefab);
            _objectPools.Add(prefab.AssetId, pool);

            return pool.Pool(parent) as T;
        }

        public void ReturnView<T>(T gameObject) where T : ViewBase
        {
            if (_objectPools.TryGetValue(gameObject.AssetId, out var pool))
            {
                pool.Return(gameObject);
            }
        }
*/
        public ViewWithModel<TViewModel> ProvideViewWithModel<TViewModel>(GameObject prefab, TViewModel viewModel,
            Transform parent = null) where TViewModel : IViewModel
        {
            var viewGo = ProvideView(prefab, parent);
            var view = viewGo.GetComponent<ViewWithModel<TViewModel>>();

            view.Initialize(viewModel);

            return view;
        }
        
        public TView ProvideViewWithModel<TViewModel, TView>(GameObject prefab, TViewModel viewModel,
            Transform parent = null)
            where TViewModel : IViewModel
            where TView : ViewWithModel<TViewModel>
        {
            var viewGo = ProvideView(prefab, parent);
            var view = viewGo.GetComponent<TView>();

            view.Initialize(viewModel);

            return view;
        }

        public void ReturnView<TViewModel>(ViewWithModel<TViewModel> view) where TViewModel : IViewModel
        {
            view.Deinit();
            ReturnView(view.gameObject);
        }
    }

    public class WindowsProvider
    {
        private readonly ViewsProvider _viewsProvider;
        private readonly WindowsViewsRegistrator _windowsViewsRegistrator;

        public WindowsProvider(List<GameObject> windowsPrefabs, ViewsProvider viewsProvider)
        {
            _viewsProvider = viewsProvider;
            _windowsViewsRegistrator = new WindowsViewsRegistrator(windowsPrefabs);
        }

        public WindowViewBase<TViewModel> ProvideWindowView<TViewModel>(TViewModel viewModel) where TViewModel : IViewModel
        {
            var prefab = _windowsViewsRegistrator.GetWindow<TViewModel>();

            var view = _viewsProvider.ProvideViewWithModel<TViewModel>(prefab.gameObject, viewModel);
            
            return view as WindowViewBase<TViewModel>;
        }
    }
}