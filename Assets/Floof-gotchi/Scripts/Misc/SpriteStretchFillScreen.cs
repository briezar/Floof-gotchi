using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteStretchFillScreen : MonoBehaviour
{
    [SerializeField] private bool _keepAspectRatio;
    [SerializeField] private bool _useMainCamera = true;

    [HideIf(nameof(_useMainCamera))]
    [SerializeField] private Camera _camera;

    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        Stretch();
    }

    [Button]
    public void Stretch()
    {
        if (_camera == null && _useMainCamera) { _camera = Camera.main; }
        if (_spriteRenderer == null) { _spriteRenderer = GetComponent<SpriteRenderer>(); }

        transform.localScale = Vector3.one;

        var topRightCorner = _camera.ViewportToWorldPoint(Vector2.one);
        var worldSpaceWidth = topRightCorner.x * 2;
        var worldSpaceHeight = topRightCorner.y * 2;

        var spriteSize = _spriteRenderer.bounds.size;

        var scaleFactorX = worldSpaceWidth / spriteSize.x;
        var scaleFactorY = worldSpaceHeight / spriteSize.y;

        if (_keepAspectRatio)
        {
            if (scaleFactorX > scaleFactorY)
            {
                scaleFactorY = scaleFactorX;
            }
            else
            {
                scaleFactorX = scaleFactorY;
            }
        }

        transform.localScale = new Vector3(scaleFactorX, scaleFactorY, 1);
    }
}