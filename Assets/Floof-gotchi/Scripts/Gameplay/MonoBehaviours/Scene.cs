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
        [field: SerializeField] public GameSceneType SceneType { get; private set; }
        [SerializeField] private SpriteRenderer _bgSpriteRenderer;
        [SerializeField] private RectTransform _moveSpace;

        public void Init()
        {
            _bgSpriteRenderer.GetComponent<SpriteStretchFillScreen>().Stretch();
        }

        public Bounds GetCameraBounds()
        {
            return _bgSpriteRenderer.bounds;
        }
        public RectTransform GetMoveSpace()
        {
            return _moveSpace;
        }
    }
}