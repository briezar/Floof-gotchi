using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using System;

public class AssetManager
{
    public static void PreloadAssetLabelRef<T>(AssetLabelReference assetLabelRef, Action<Dictionary<string, T>> onLoaded) where T : Component
    {
        Utils.StartCoroutine(PreloadAssetLabelRefRoutine<T>(assetLabelRef, onLoaded));
    }
    private static IEnumerator PreloadAssetLabelRefRoutine<T>(AssetLabelReference assetLabelRef, Action<Dictionary<string, T>> onLoaded) where T : Component
    {
        var preloadedAssets = new Dictionary<string, T>();
        var loadResourceLocationsHandle = Addressables.LoadResourceLocationsAsync(assetLabelRef, typeof(GameObject));

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

        if (!groupOp.IsDone) { yield return groupOp; }

        Addressables.Release(loadResourceLocationsHandle);

        foreach (var item in preloadedAssets)
        {
            Debug.Log(item.Key + " - " + item.Value.name);
        }

        onLoaded?.Invoke(preloadedAssets);
    }

    public static void LoadAsync<T>(string path, Action<T> onComplete = null) where T : Component
    {
        var handle = Addressables.LoadAssetAsync<T>(path);
        handle.Completed += (opHandle) => { onComplete?.Invoke(opHandle.Result.GetComponent<T>()); };
    }

    public static void InstantiateAsync<T>(string path, Transform parent = null, Action<T> onComplete = null) where T : Component
    {
        var handle = Addressables.InstantiateAsync(path, parent);
        handle.Completed += (opHandle) => { onComplete?.Invoke(opHandle.Result.GetComponent<T>()); };
    }

}
