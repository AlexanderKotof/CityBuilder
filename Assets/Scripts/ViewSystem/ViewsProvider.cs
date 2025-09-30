using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace ViewSystem
{
    public class ViewsProvider : IViewsProvider
    {
        private readonly Dictionary<string, GameObjectPool> _gameObjectPools = new();
        private readonly Dictionary<Type, ComponentObjectPool> _componentsPools = new();
        
        public async Task<GameObject> ProvideViewAsync(string assetKey, [CanBeNull] Transform parent = null)
        {
            if (_gameObjectPools.TryGetValue(assetKey, out var pool))
            {
                return await pool.Pool(parent);
            }

            pool = new GameObjectPool(assetKey);
            
            _gameObjectPools.Add(assetKey, pool);

            return await pool.Pool(parent);
        }

        public async Task<T> ProvideViewAsync<T>(string assetKey, Transform parent = null)
            where T : Component
        {
            Type type = typeof(T);
            if (_componentsPools.TryGetValue(type, out var pool))
            {
                return await pool.Pool(parent) as T;
            }

            pool = new ComponentObjectPool(assetKey);
            
            _componentsPools.Add(type, pool);

            return await pool.Pool(parent) as T;
        }
        
        /// <summary>
        /// Not work for now!!!
        /// </summary>
        /// <param name="gameObject"></param>
        public void ReturnView(GameObject gameObject)
        {
            if (_gameObjectPools.TryGetValue(GetAssetKey(gameObject), out var pool))
            {
                Debug.Log("Successfully returned a gameobject view");
                pool.Return(gameObject);
            }
        }

        private string GetAssetKey(GameObject gameObject)
        {
            return string.Empty;
        }

        public void ReturnView(Component component)
        {
            if (_componentsPools.TryGetValue(component.GetType(), out var pool))
            {
                Debug.Log("Successfully returned a component view");
                pool.Return(component);
            }
        }
    }
}