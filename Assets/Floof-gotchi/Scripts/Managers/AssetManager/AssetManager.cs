using System;
using System.Collections;
using System.Collections.Generic;
using Dre0Dru.AddressableAssets.Loaders;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Floof
{
    public class AssetManager
    {
        public static readonly IAssetsReferenceLoader<GameObject> PrefabLoader = new AssetsReferenceLoader<GameObject>();
        public static readonly IAssetsReferenceLoader<Sprite> SpriteLoader = new AssetsReferenceLoader<Sprite>();

        public static List<string> GetKeys(params AssetLabelReference[] labelReferences)
        {
            var locationHandle = Addressables.LoadResourceLocationsAsync(labelReferences as IEnumerable, Addressables.MergeMode.Union, typeof(GameObject));
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
    }


    [Serializable]
    public class PrefabReference<T> : AssetReferenceGameObject where T : Component
    {
        public PrefabReference(string guid) : base(guid) { }
    }

    [Serializable]
    public class PrefabReference : AssetReferenceGameObject
    {
        public PrefabReference(string guid) : base(guid) { }
    }
}