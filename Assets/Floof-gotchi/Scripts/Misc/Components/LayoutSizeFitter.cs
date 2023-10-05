using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Floof
{
    public class LayoutSizeFitter : UIBehaviour
    {
        [Serializable]
        private class SizeInfo
        {
            [Min(0)] public int childIndex;
            [Min(-1)] public int maxHeight = -1;
            [Min(-1)] public int maxWidth = -1;
        }

        [SerializeField] private SizeInfo[] _childSizeInfo;

        private RectTransform rectTransform => transform as RectTransform;

        private bool _isChangingSize;


        private void CalculateSize()
        {
            var activeChildren = transform.GetChildList(false);

            var availableHeight = rectTransform.rect.height;
            var availableWidth = rectTransform.rect.width;

            var setIndexes = new HashSet<int>();

            foreach (var sizeInfo in _childSizeInfo)
            {
                var childIndex = sizeInfo.childIndex;
                if (childIndex >= activeChildren.Count) { continue; }

                var child = activeChildren[childIndex] as RectTransform;
                setIndexes.Add(childIndex);

                var maxHeight = sizeInfo.maxHeight;
                if (maxHeight > 0)
                {
                    child.SetHeight(maxHeight);
                    availableHeight -= maxHeight;
                }

                var maxWidth = sizeInfo.maxWidth;
                if (maxWidth > 0)
                {
                    child.SetWidth(maxWidth);
                    availableWidth -= maxWidth;
                }
            }

            var remainingChildCount = activeChildren.Count - _childSizeInfo.Length;

            var heightPerRemainingChild = availableHeight / remainingChildCount;
            var widthPerRemainingChild = availableWidth / remainingChildCount;

            for (int i = 0; i < activeChildren.Count; i++)
            {
                if (setIndexes.Contains(i)) { continue; }

                var child = activeChildren[i] as RectTransform;
                child.SetHeight(heightPerRemainingChild);
                child.SetWidth(widthPerRemainingChild);
            }
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (Application.isPlaying) { return; }
            CalculateSize();
        }
#endif

        protected override void OnRectTransformDimensionsChange()
        {
            if (_isChangingSize) return;
            _isChangingSize = true;
            CalculateSize();
            _isChangingSize = false;
        }

        protected override void Start()
        {
            CalculateSize();
        }

    }
}