using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameSystems.Common.ViewSystem.Pools;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameSystems.Common.ViewSystem.ViewsProvider
{
    public class ViewsProvider : IViewsProvider, IDisposable
    {
        private readonly Dictionary<string, IObjectPool> _viewPools = new();
        private readonly Dictionary<int, string> _objectsIdsToPools = new();
        
        public void Dispose()
        {
            foreach (var pool in _viewPools.Values)
            {
                while (pool.TryPool(out Object poolObject))
                {
                    AssetsProvider.AssetsProvider.Release(poolObject);
                }
            }
            
            _viewPools.Clear();
            _objectsIdsToPools.Clear();
        }
        
        public async UniTask<GameObject> ProvideViewAsync(string assetKey, Transform parent = null)
        {
            var pool = GetPool<GameObject>(assetKey);
            if (pool.TryPool(out GameObject gameObject))
            {
                SetViewActive(gameObject, true);
                return gameObject;
            }
            
            var result = await CreateView(assetKey, parent);
            _objectsIdsToPools.Add(result.GetInstanceID(), assetKey);
            return result;
        }
        
        public async UniTask<T> ProvideViewAsync<T>(string assetKey, Transform parent = null) where T : Component
        {
            var pool = GetPool<T>(assetKey);
            if (pool.TryPool(out T gameObject))
            {
                SetViewActive(gameObject, true);
                return gameObject;
            }
            
            var result = await CreateView<T>(assetKey, parent);
            _objectsIdsToPools.Add(result.GetInstanceID(), assetKey);
            return result;
        }
        
        private ObjectPool<T> GetPool<T>(string assetKey) where T : Object
        {
            if (_viewPools.TryGetValue(assetKey, out IObjectPool pool))
            {
                return pool as ObjectPool<T>;  
            }
            
            var p = new ObjectPool<T>();
            _viewPools.Add(assetKey, p);
            return p;
        }
        
        public void ReturnView<T>(T component) where T : Object
        {
            if (_objectsIdsToPools.TryGetValue(component.GetInstanceID(), out string poolId) == false)
            {
                Debug.LogError($"Cannot release object of type {component.GetType().Name}");
                return;
            }

            if (_viewPools.TryGetValue(poolId, out IObjectPool pool) == false)
            {
                Debug.LogError($"No pool found for {component.GetType().Name}");
                return;
            }
            
            SetViewActive(component, false);
            pool.Return(component);
        }

        private async Task<T> CreateView<T>(string assetKey, Transform parent = null)
        {
            var gameObject = await CreateView(assetKey, parent);
            return gameObject.GetComponent<T>();
        }
        
        private async Task<GameObject> CreateView(string assetKey, Transform parent = null)
        {
            var gameObject = await AssetsProvider.AssetsProvider.InstantiateAsync(assetKey, parent);
            return gameObject;
        }

        private static void SetViewActive<T>(T view, bool isActive) where T : Object
        {
            switch (view)
            {
                case GameObject gameObject:
                    gameObject.SetActive(isActive);
                    break;
                case Component component:
                    component.gameObject.SetActive(isActive);
                    break;
                
                default:
                    Debug.LogError($"Unprocessed view object type {typeof(T).Name}");
                    break;
            }
        }
    }
}