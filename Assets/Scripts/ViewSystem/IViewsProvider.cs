using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace ViewSystem
{
    public interface IViewsProvider
    {
        public Task<GameObject> ProvideViewAsync(string assetKey, [CanBeNull] Transform parent = null);
        public Task<T> ProvideViewAsync<T>(string assetKey, [CanBeNull] Transform parent = null) where T : Component;

        public void ReturnView(Component component);
    }
}