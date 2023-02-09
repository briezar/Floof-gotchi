using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AutoScroll : MonoBehaviour
{
    [SerializeField] private RectTransform _boundParent;
    [SerializeField] private RectTransform _target;
    [SerializeField] private float _widthHeightRatio;
    [SerializeField] private float _duration = 10f;

    private void Start()
    {
        _target.anchoredPosition = Vector3.zero;
        var minLocalPosX = -(_boundParent.rect.height * _widthHeightRatio - _boundParent.rect.width - 1f);
        _target.DOAnchorPosX(minLocalPosX, _duration).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

}
