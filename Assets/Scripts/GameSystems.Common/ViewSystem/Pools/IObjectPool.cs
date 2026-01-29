using UnityEngine;

namespace ViewSystem
{
    public interface IObjectPool
    {
        bool TryPool(out Object obj);
        void Return(Object obj);
    }
}