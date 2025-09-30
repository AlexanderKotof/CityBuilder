using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AssetsProvider
{
    public static class AssetsProvider
    {
        public static async Task<T> GetAssetAsync<T>(string assetKey)
            where T : UnityEngine.Object
        {
            return await Addressables.LoadAssetAsync<T>(assetKey).Task;
        }
        
        public static async Task<GameObject> InstantiateAsync(string assetKey, Transform parent = null)
        {
            return await Addressables.InstantiateAsync(assetKey, parent).Task;
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