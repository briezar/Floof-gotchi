using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Floof
{
    public class Scene : MonoBehaviour
    {
        [field: SerializeField] public RectTransform MoveSpace { get; private set; }
        [SerializeField] private SpriteRenderer _bgSpriteRenderer;
        [SerializeField] private CameraFollow _camFollow;

        public void Setup(Transform floofTransform)
        {
            _bgSpriteRenderer.GetComponent<SpriteStretchFillScreen>().Stretch();
            _camFollow.SetTarget(floofTransform);

            var minPos = _bgSpriteRenderer.bounds.min;
            var maxPos = _bgSpriteRenderer.bounds.max;
            _camFollow.SetBounds(minPos, maxPos);
        }


    }
}