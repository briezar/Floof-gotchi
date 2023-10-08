using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Floof
{
    public struct Address
    {
        public IReadOnlyList<object> Keys => _keyList;
        private List<object> _keyList;

        public Address(params string[] paths)
        {
            _keyList = new List<object>();
            AddPath(paths);
        }
        public Address(params IKeyEvaluator[] keys)
        {
            _keyList = new List<object>();
            AddKey(keys);
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
