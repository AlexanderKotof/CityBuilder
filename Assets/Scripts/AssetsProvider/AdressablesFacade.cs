using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AssetsProvider
{
    public static class AdressablesFacade
    {
        public static UniTask<T> GetAssetAsync<T>(string assetKey)
            where T : UnityEngine.Object
        {
            return Addressables.LoadAssetAsync<T>(assetKey).ToUniTask();
        }
        
        public static UniTask<GameObject> InstantiateAsync(string assetKey, Transform parent = null)
        {
            return Addressables.InstantiateAsync(assetKey, parent).ToUniTask();
        }
        
        public static void Release(UnityEngine.Object asset)
        {
            Addressables.Release(asset);
        }
    }

    public static class AssetReferenceExtensions
    {
        public static string GetAssetKey(this AssetReference assetReference)
        {
            if (assetReference.IsValid() == false)
            {
                Debug.LogError("AssetReference is invalid!", assetReference.Asset);
                return string.Empty;
            }

            return assetReference.RuntimeKey.ToString();
        }
    }
}