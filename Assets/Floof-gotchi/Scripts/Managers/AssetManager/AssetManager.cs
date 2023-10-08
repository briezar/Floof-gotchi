using System;
using System.Collections;
using System.Collections.Generic;
using Dre0Dru.AddressableAssets.Loaders;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Floof
{
    public class AssetManager
    {
        public static readonly IAssetsReferenceLoader<GameObject> PrefabLoader = new AssetsReferenceLoader<GameObject>();

        public static List<string> GetKeys(Address address)
        {
            var locationHandle = Addressables.LoadResourceLocationsAsync(address.Keys, Addressables.MergeMode.Union, typeof(GameObject));
            locationHandle.WaitForCompletion();
            var locations = locationHandle.Result;

            var keys = new List<string>();

            foreach (var location in locations)
            {
                keys.Add(location.PrimaryKey);
            }
            Addressables.Release(locationHandle);

            return keys;
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

        public T Component => Asset as T;
    }

    [Serializable]
    public class PrefabReference : AssetReferenceGameObject
    {
        public PrefabReference(string guid) : base(guid) { }
    }
}