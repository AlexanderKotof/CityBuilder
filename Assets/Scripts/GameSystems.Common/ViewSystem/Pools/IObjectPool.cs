using UnityEngine;

namespace GameSystems.Common.ViewSystem.Pools
{
    public interface IObjectPool
    {
        bool TryPool(out Object obj);
        void Return(Object obj);
    }
}