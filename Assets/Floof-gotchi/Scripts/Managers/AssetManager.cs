using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class AssetManager
{
    public static Coroutine PreloadAssetLabelRef<T>(AssetLabelReference[] assetLabelRefs, Action<Dictionary<string, T>> onLoaded, Action<float> actionPercentComplete = null) where T : Component
    {
        var labels = new HashSet<string>();
        foreach (var labelRef in assetLabelRefs)
        {
            labels.Add(labelRef.labelString);
        }
        return Utils.StartCoroutine(PreloadAssetLabelRefRoutine());
        IEnumerator PreloadAssetLabelRefRoutine()
        {
            var preloadedAssets = new Dictionary<string, T>();
            var loadResourceLocationsHandle = Addressables.LoadResourceLocationsAsync(labels, Addressables.MergeMode.Intersection, typeof(GameObject));

            if (!loadResourceLocationsHandle.IsDone) { yield return loadResourceLocationsHandle; }

            //start each location loading
            var operationList = new List<AsyncOperationHandle>();

            foreach (var location in loadResourceLocationsHandle.Result)
            {
                var loadAssetHandle = Addressables.LoadAssetAsync<GameObject>(location);
                loadAssetHandle.Completed += (obj) => { preloadedAssets.Add(obj.Result.name, obj.Result.GetComponent<T>()); };
                operationList.Add(loadAssetHandle);
            }

            //create a GroupOperation to wait on all the above loads at once. 
            var groupOp = Addressables.ResourceManager.CreateGenericGroupOperation(operationList);

            while (!groupOp.IsDone)
            {
                actionPercentComplete?.Invoke(groupOp.PercentComplete);
                yield return null;
            }
            actionPercentComplete?.Invoke(1f);

            Addressables.Release(loadResourceLocationsHandle);

            foreach (var item in preloadedAssets)
            {
                Debug.Log($"Preload ID: {item.Key} => {item.Value.name}");
            }

            onLoaded?.Invoke(preloadedAssets);
        }
    }

    public static AsyncOperationHandle CreateGroupOperation(params AsyncOperationHandle[] handles)
    {
        return Addressables.ResourceManager.CreateGenericGroupOperation(handles.ToList());
    }

    public static AsyncOperationHandle LoadTextAsync(string path, Action<string> onComplete = null)
    {
        var handle = Addressables.LoadAssetAsync<TextAsset>(path);
        handle.Completed += (opHandle) =>
        {
            onComplete?.Invoke(opHandle.Result.text);
            Addressables.Release(opHandle);
        };
        return handle;
    }

    public static void LoadAssetsAsync<T>(IList<AssetReference> keys, Action<Dictionary<string, T>> onLoaded) where T : UnityEngine.Object
    {
        Utils.StartCoroutine(LoadKeysRoutine());
        IEnumerator LoadKeysRoutine()
        {
            var locationsHandle = Addressables.LoadResourceLocationsAsync(keys, Addressables.MergeMode.Union, typeof(T));

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

    public static void LoadAsync<T>(string path, Action<T> onComplete = null) where T : Component
    {
        var handle = Addressables.LoadAssetAsync<T>(path);
        handle.Completed += (opHandle) => { onComplete?.Invoke(opHandle.Result.GetComponent<T>()); };
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
            if (handle.Result.TryGetComponent<T>(out var instantiatedAsset))
            {
                onComplete?.Invoke(instantiatedAsset);
                return;
            }
            Debug.LogError($"Missing or incorrect component: {typeof(T).Name}");
        }
    }

}
