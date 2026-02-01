using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;

namespace CityBuilder.GameSystems.Common.ViewSystem.ViewsProvider
{
    public interface IViewsProvider
    {
        UniTask<GameObject> ProvideViewAsync(string assetKey, [CanBeNull] Transform parent = null);
        UniTask<T> ProvideViewAsync<T>(string assetKey, [CanBeNull] Transform parent = null) where T : Component;
        void ReturnView<T>(T component) where T : Object;
    }
}