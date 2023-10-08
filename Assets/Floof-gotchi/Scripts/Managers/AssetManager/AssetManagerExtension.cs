using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Dre0Dru.AddressableAssets.Loaders;
using UnityEngine;
using Object = UnityEngine.Object;
using UnityEngine.AddressableAssets;

namespace Floof
{
    public static class AssetManagerExtension
    {
        public static T GetComponent<T>(this IAssetsReferenceLoader<GameObject> assetsLoader, PrefabReference prefabReference) where T : Component
        {
            T component = null;
            if (assetsLoader.TryGetAsset(prefabReference, out var gameObject))
            {
                gameObject.TryGetComponent<T>(out component);
                return component;
            }

            return component;
        }

        public static T GetComponent<T>(this IAssetsReferenceLoader<GameObject> assetsLoader, PrefabReference<T> prefabReference) where T : Component
        {
            return GetComponent<T>(assetsLoader, prefabReference);
        }

        public static UniTask PreloadAssetsAsync<TKey, TAsset>(this IAssetsLoader<TKey, TAsset> assetsLoader, params TKey[] keys) where TAsset : Object
        {
            return UniTask.WhenAll(keys.Select(assetsLoader.PreloadAssetAsync));
        }

        public static UniTask PreloadAssetsAsync<TKey, TAsset>(this IAssetsLoader<TKey, TAsset> assetsLoader, IEnumerable<TKey> keys) where TAsset : Object
        {
            return UniTask.WhenAll(keys.Select(assetsLoader.PreloadAssetAsync));
        }

        public static void UnloadAssets<TKey, TAsset>(this IAssetsLoader<TKey, TAsset> assetsLoader, params TKey[] keys) where TAsset : Object
        {
            assetsLoader.UnloadAssets((IEnumerable<TKey>)keys);
        }

        public static void UnloadAssets<TKey, TAsset>(this IAssetsLoader<TKey, TAsset> assetsLoader, IEnumerable<TKey> keys) where TAsset : Object
        {
            foreach (var key in keys)
            {
                assetsLoader.UnloadAsset(key);
            }
        }

        public static UniTask<TAsset> LoadAssetAsync<TAsset>(this IAssetsReferenceLoader<TAsset> assetsLoader, Address address) where TAsset : Object
        {
            string key;
            if (address.Keys.Count > 1)
            {
                key = AssetManager.GetLocations(address, typeof(TAsset))[0].PrimaryKey;
            }
            else
            {
                key = (string)address.Keys[0];
            }
            return assetsLoader.LoadAssetAsync(new AssetReferenceT<TAsset>(key));
        }

        public static UniTask<TAsset[]> LoadAssetsAsync<TKey, TAsset>(this IAssetsLoader<TKey, TAsset> assetsLoader, params TKey[] keys) where TAsset : Object
        {
            return LoadAssetsAsync(assetsLoader, (IEnumerable<TKey>)keys);
        }

        public static UniTask<TAsset[]> LoadAssetsAsync<TKey, TAsset>(this IAssetsLoader<TKey, TAsset> assetsLoader, IEnumerable<TKey> keys) where TAsset : Object
        {
            return UniTask.WhenAll(keys.Select(assetsLoader.LoadAssetAsync));
        }

        // public static IEnumerable<TAsset> GetAssets<TKey, TAsset>(this IAssetsLoader<TKey, TAsset> assetsLoader, params TKey[] keys) where TAsset : Object
        // {
        //     return assetsLoader.GetAssets((IEnumerable<TKey>)keys);
        // }

        // public static IEnumerable<TAsset> GetAssets<TKey, TAsset>(this IAssetsLoader<TKey, TAsset> assetsLoader, IEnumerable<TKey> keys) where TAsset : Object
        // {
        //     return keys.Select(assetsLoader.GetAsset);
        // }

        // public static bool TryGetAssets<TKey, TAsset>(this IAssetsLoader<TKey, TAsset> assetsLoader, out IEnumerable<TAsset> assets, params TKey[] keys) where TAsset : Object
        // {
        //     return assetsLoader.TryGetAssets(keys, out assets);
        // }

        // public static bool TryGetAssets<TKey, TAsset>(this IAssetsLoader<TKey, TAsset> assetsLoader, IEnumerable<TKey> keys, out IEnumerable<TAsset> assets) where TAsset : Object
        // {
        //     if (keys.All(assetsLoader.IsAssetLoaded) == false)
        //     {
        //         assets = Enumerable.Empty<TAsset>();
        //         return false;
        //     }

        //     assets = assetsLoader.GetAssets(keys);
        //     return true;
        // }
    }
}