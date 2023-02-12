using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Image _background;
    [SerializeField] private RectTransform _bound;

    private RectTransform _bgRectTransform;
    private float _xMin, _xMax;

    private void Awake()
    {
        _bgRectTransform = (RectTransform)_background.transform;
        var ratio = _background.sprite.rect.width / _background.sprite.rect.height;

        _xMax = (_bound.rect.height * ratio - _bound.rect.width - 1f) / 2;
        _xMin = -_xMax;
    }

    private void Update()
    {
        if (IsWithinBounds() || (!IsWithinBounds() && Mathf.Abs(_target.localPosition.x) < Mathf.Abs(_bgRectTransform.localPosition.x)))
        {
            _bgRectTransform.localPosition = new Vector3(-_target.localPosition.x, 0);
        }
    }

    private bool IsWithinBounds()
    {
        return (_bgRectTransform.localPosition.x >= _xMin && _bgRectTransform.localPosition.x <= _xMax);
    }

}
