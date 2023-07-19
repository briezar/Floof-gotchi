using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class AssetManager
{
    public static void LoadAssetsByLabel<T>(List<AssetLabelReference> assetLabelRefs, Action<List<T>> onLoaded, Action<float> actionPercentComplete = null) where T : UnityEngine.Object
    {
        Utils.StartCoroutine(LoadRoutine());

        IEnumerator LoadRoutine()
        {
            var isComponent = typeof(Component).IsAssignableFrom(typeof(T));
            var loadedAssets = new List<T>();
            var loadResourceLocationsHandle = Addressables.LoadResourceLocationsAsync(assetLabelRefs, Addressables.MergeMode.Intersection, isComponent ? typeof(GameObject) : typeof(T));

            if (!loadResourceLocationsHandle.IsDone) { yield return loadResourceLocationsHandle; }

            //start each location loading
            var operationList = new List<AsyncOperationHandle>();

            AsyncOperationHandle loadAssetHandle;

            foreach (var location in loadResourceLocationsHandle.Result)
            {
                if (isComponent)
                {
                    loadAssetHandle = LoadAssetByLabel<GameObject>(location, (prefab) => loadedAssets.Add(prefab.GetComponent<T>()));
                }
                else
                {
                    loadAssetHandle = LoadAssetByLabel<T>(location, (asset) => loadedAssets.Add(asset));
                }

                operationList.Add(loadAssetHandle);
            }

            //create a GroupOperation to wait on all the above loads at once. 
            var groupOp = Addressables.ResourceManager.CreateGenericGroupOperation(operationList, true);

            while (!groupOp.IsDone)
            {
                actionPercentComplete?.Invoke(groupOp.PercentComplete);
                yield return null;
            }
            actionPercentComplete?.Invoke(1);

            Addressables.Release(loadResourceLocationsHandle);

            foreach (var item in loadedAssets)
            {
                Debug.Log($"Loaded: {item.name}");
            }

            onLoaded?.Invoke(loadedAssets);
        }
    }

    public static void LoadAssetsAsync<T>(IList<AssetReference> assetRefs, Action<Dictionary<string, T>> onLoaded) where T : UnityEngine.Object
    {
        Utils.StartCoroutine(LoadRoutine());
        IEnumerator LoadRoutine()
        {
            var locationsHandle = Addressables.LoadResourceLocationsAsync(assetRefs, Addressables.MergeMode.Union);

            if (!locationsHandle.IsDone) { yield return locationsHandle; }

            var loadedAssets = new Dictionary<string, T>();

            var resultsHandle = Addressables.LoadAssetsAsync<T>(locationsHandle.Result, (asset) => loadedAssets.Add(asset.name, asset));
            if (!resultsHandle.IsDone) { yield return resultsHandle; }

            Addressables.Release(locationsHandle);

            foreach (var item in loadedAssets)
            {
                Debug.Log($"Preload ID {item.Key} => {item.Value.name}");
            }

            onLoaded?.Invoke(loadedAssets);
        }
    }

    public static AsyncOperationHandle LoadAssetByLabel<T>(IResourceLocation location, Action<T> onComplete = null) where T : UnityEngine.Object
    {
        var handle = Addressables.LoadAssetAsync<T>(location);
        handle.Completed += (opHandle) => { onComplete?.Invoke(opHandle.Result); };
        return handle;
    }

    public static AsyncOperationHandle LoadAssetByPath<T>(string path, Action<T> onComplete = null) where T : UnityEngine.Object
    {
        var handle = Addressables.LoadAssetAsync<T>(path);
        handle.Completed += (opHandle) => { onComplete?.Invoke(opHandle.Result); };
        return handle;
    }

    public static T Instantiate<T>(string path, Transform parent = null) where T : Component
    {
        var gameObject = Addressables.InstantiateAsync(path, parent).WaitForCompletion();
        if (gameObject == null)
        {
            Debug.LogWarning(gameObject.name + " is null");
            return null;
        }

        if (!gameObject.TryGetComponent<T>(out var component))
        {
            Debug.LogWarning("GameObject does not contain " + typeof(T).Name);
        }
        return component;
    }

    public static AsyncOperationHandle InstantiateAsync<T>(string path, Transform parent = null, Action<T> onComplete = null) where T : Component
    {
        var handle = Addressables.InstantiateAsync(path, parent);

        if (handle.IsDone)
        {
            Completed(handle);
        }
        else
        {
            handle.Completed += Completed;
        }

        return handle;

        void Completed(AsyncOperationHandle<GameObject> handle)
        {
            if (!handle.Result.TryGetComponent<T>(out var instantiatedAsset))
            {
                Debug.LogError($"Missing or incorrect component: {typeof(T).Name}");
            }
            onComplete?.Invoke(instantiatedAsset);
        }
    }

}
