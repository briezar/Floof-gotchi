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

        private SpriteStretchFillScreen _spriteStretchFillScreen;

        public Bounds GetBounds()
        {
            _spriteStretchFillScreen ??= _bgSpriteRenderer.GetComponent<SpriteStretchFillScreen>();
            _spriteStretchFillScreen.Stretch();
            return _bgSpriteRenderer.bounds;
        }
    }
}