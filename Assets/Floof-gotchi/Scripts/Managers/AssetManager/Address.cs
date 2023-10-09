using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Floof
{
    public struct Address
    {
        public Addressables.MergeMode MergeMode;
        private List<object> _keyList;

        public string Key => (string)Keys[0];
        public IReadOnlyList<object> Keys => _keyList;

        public Address(params string[] paths)
        {
            _keyList = new List<object>();
            MergeMode = Addressables.MergeMode.Intersection;
            AddPath(paths);
        }
        public Address(params IKeyEvaluator[] keys)
        {
            _keyList = new List<object>();
            MergeMode = Addressables.MergeMode.Intersection;
            AddKey(keys);
        }

        public Address SetMergeMode(Addressables.MergeMode mergeMode)
        {
            MergeMode = mergeMode;
            return this;
        }

        public Address AddPath(params string[] paths)
        {
            _keyList.AddRange(paths);
            return this;
        }

        /// <summary> AssetReference or AssetLabelReference </summary>
        public Address AddKey(params IKeyEvaluator[] keys)
        {
            _keyList.AddRange(keys);
            return this;
        }

        public override string ToString()
        {
            var stringList = new List<string>();

            foreach (var item in _keyList)
            {
                var key = string.Empty;

                switch (item)
                {
                    case AssetLabelReference assetLabel:
                        key = assetLabel.labelString;
                        break;
                    case AssetReference assetRef:
                        key = assetRef.RuntimeKey.ToString();
#if UNITY_EDITOR
                        key = assetRef.editorAsset.name;
#endif
                        break;
                    case string:
                        key = item.ToString();
                        break;
                }

                stringList.Add(key);
            }

            return stringList.JoinElements();
        }

    }
}
