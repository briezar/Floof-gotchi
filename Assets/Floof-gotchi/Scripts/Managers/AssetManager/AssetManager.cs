using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Dre0Dru.AddressableAssets.Loaders;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using Object = UnityEngine.Object;

namespace Floof
{
    public class AssetManager
    {
        public static readonly IAssetsReferenceLoader<GameObject> PrefabLoader = new AssetsReferenceLoader<GameObject>();
        public static readonly IAssetsReferenceLoader<ScriptableObject> ScriptableObjectLoader = new AssetsReferenceLoader<ScriptableObject>();

        public static IList<IResourceLocation> GetLocations(Address address, Type type = null)
        {
            var locationHandle = Addressables.LoadResourceLocationsAsync(address.Keys, Addressables.MergeMode.Intersection, type);
            var locations = locationHandle.WaitForCompletion();
            Addressables.Release(locationHandle);

            return locations;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Init()
        {
            Addressables.InitializeAsync();
        }
    }


    [Serializable]
    public class ScriptableObjectReference<T> : AssetReferenceT<ScriptableObject> where T : ScriptableObject
    {
        public ScriptableObjectReference(string guid) : base(guid) { }
        public T Component => Asset as T;
    }

    [Serializable]
    public class PrefabReference<T> : PrefabReference where T : Component
    {
        public PrefabReference(string guid) : base(guid) { }


        public T Component => (Asset as GameObject).GetComponent<T>();
    }

    [Serializable]
    public class PrefabReference : AssetReferenceGameObject
    {
        public PrefabReference(string guid) : base(guid) { }
    }
}