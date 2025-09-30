using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace ViewSystem
{
    public interface IViewsProvider
    {
        Task<GameObject> ProvideViewAsync(string assetKey, [CanBeNull] Transform parent = null);
        Task<T> ProvideViewAsync<T>(string assetKey, [CanBeNull] Transform parent = null) where T : Component;
        void ReturnView<T>(T component) where T : Object;
    }
}