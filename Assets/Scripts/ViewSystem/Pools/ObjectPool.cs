using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ViewSystem
{
    public class ObjectPool<T> : ObjectsPoolBase
        where T : Object
    {
        public bool TryPool([CanBeNull] out T result)
        {
            if (base.TryPool(out var obj))
            {
                result = obj as T;
                OnPool(result);
                return true;
            }
            result = default;
            return false;
        }
        
        public void Return(T gameobject)
        {
            base.Return(gameobject);
            OnReturn(gameobject);
        }
        
        protected virtual void OnPool(T gameobject) 
        {
        }
        protected virtual void OnReturn(T gameobject)
        {
        }
    }
}