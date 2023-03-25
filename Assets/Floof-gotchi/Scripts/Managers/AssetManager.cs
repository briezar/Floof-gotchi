using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using System;

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
                Debug.Log($"Preload ID {item.Key} => {item.Value.name}");
            }

            onLoaded?.Invoke(preloadedAssets);
        }
    }

    public static void LoadAsync<T>(string path, Action<T> onComplete = null) where T : Component
    {
        var handle = Addressables.LoadAssetAsync<T>(path);
        handle.Completed += (opHandle) => { onComplete?.Invoke(opHandle.Result.GetComponent<T>()); };
    }

    public static void InstantiateAsync<T>(string path, Transform parent = null, Action<T> onComplete = null) where T : Component
    {
        var handle = Addressables.InstantiateAsync(path, parent);
        handle.Completed += (opHandle) =>
        {
            try
            {
                onComplete?.Invoke(opHandle.Result.GetComponent<T>());
            }
            catch (NullReferenceException)
            {
                Debug.LogError($"Missing or incorrect component: {typeof(T).Name}");
            }
        };
    }

}
