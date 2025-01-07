using System.Collections.Generic;
using UnityEngine;

namespace ViewSystem
{
    public class ObjectsPool<T> where T : Object
    {
        private readonly T _prefabReference;

        private readonly Queue<T> _pool = new Queue<T>();

        public ObjectsPool(T prefabReference)
        {
            _prefabReference = prefabReference;
        }

        public T Pool(Transform parent = null)
        {
            if (_pool.Count > 0)
            {
                var go = _pool.Dequeue();
                OnPool(go);
                return go;
            }

            var gameObject = Object.Instantiate(_prefabReference, parent);
            gameObject.name = _prefabReference.name;
            OnPool(gameObject);
            return gameObject;
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
    }
}