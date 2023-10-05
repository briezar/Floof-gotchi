using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Dre0Dru.AddressableAssets.Loaders;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Floof
{
    public static class AssetManagerExtension
    {
        public static bool TryGetComponent<T>(this IAssetsReferenceLoader<GameObject> assetsLoader, PrefabReference prefabReference, out T component) where T : Component
        {
            if (assetsLoader.TryGetAsset(prefabReference, out var gameObject))
            {
                return gameObject.TryGetComponent(out component);
            }

            component = null;
            return false;
        }
    
        public static bool TryGetComponent<T>(this IAssetsReferenceLoader<GameObject> assetsLoader, PrefabReference<T> prefabReference, out T component) where T : Component
        {
            if (assetsLoader.TryGetAsset(prefabReference, out var gameObject))
            {
                return gameObject.TryGetComponent(out component);
            }

            component = null;
            return false;
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