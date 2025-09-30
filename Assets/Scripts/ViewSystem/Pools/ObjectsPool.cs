using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ViewSystem
{
    public interface IObjectPool<T>
    {
        bool TryPool(out T? obj);
        void Return(T obj);
    }
    
    public class ObjectsPoolBase<T> : IObjectPool<T> where T : Object
    {
        private readonly Queue<T> _pool = new();
        
        public bool TryPool([CanBeNull] out T obj)
        {
            if (_pool.Count > 0)
            {
                var go = _pool.Dequeue();
                obj = go;
                OnPool(go);
                return true;
            }

            obj = default;
            return false;
        }
        
        public void Return(T gameobject)
        {
            _pool.Enqueue(gameobject);
            OnReturn(gameobject);
        }
        
        protected virtual void OnPool(T gameobject) 
        {
        }
       
        protected virtual void OnReturn(T gameobject)
        {
        }
    }
    
    public class NewObjectsPool<T> : IDisposable where T : Object
    {
        private readonly string _assetKey;
        private readonly Queue<T> _pool = new Queue<T>();

        public NewObjectsPool(string assetKey)
        {
            _assetKey = assetKey;
        }

        public async Task<T> Pool(Transform parent = null)
        {
            if (_pool.Count > 0)
            {
                var go = _pool.Dequeue();
                OnPool(go);
                return go;
            }

            var gameObject = await AssetsProvider.AssetsProvider.InstantiateAsync(_assetKey, parent);
            var result = gameObject.GetComponent<T>();
            
            Debug.LogError($"Instantiated object {gameObject?.name ?? "EMPTY"}, result component {result?.GetType().Name ?? "NONE"}, (requested type {typeof(T).Name})");
            
            OnPool(result);
            return result;
        }

        protected virtual void OnPool(T gameobject) 
        {
            
        }

        public void Return(T gameobject)
        {
            _pool.Enqueue(gameobject);
            OnReturn(gameobject);
        }

        protected virtual void OnReturn(T gameobject)
        {
        }

        public void Dispose()
        {
            foreach (var obj in _pool)
            {
                AssetsProvider.AssetsProvider.Release(obj);
            }
            _pool.Clear();
        }
    }
}