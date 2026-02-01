using System.Collections.Generic;
using UnityEngine;

namespace CityBuilder.GameSystems.Common.ViewSystem.Pools
{
    public class ObjectsPoolBase : IObjectPool 
    {
        private readonly Queue<Object> _pool = new();
        
        public bool TryPool(out Object obj)
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

        public void Return(Object obj)
        {
            if (_pool.Contains(obj))
                return;
            
            _pool.Enqueue(obj);
            OnReturn(obj);
        }
        
        protected virtual void OnPool(Object gameobject) 
        {
        }
        protected virtual void OnReturn(Object gameobject)
        {
        }
    }
}