using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Loading : MonoBehaviour
{
    [SerializeField] private Transform _loadingSpinLeft, _loadingSpinRight;
    [field: SerializeField] public TextMeshProUGUI Text { get; private set; }

    private void OnEnable()
    {
        _loadingSpinLeft.DOLocalRotate(new Vector3(0, 0, 360), 2, RotateMode.LocalAxisAdd).SetLoops(-1).SetEase(Ease.Linear).SetId(gameObject.GetInstanceID());
        _loadingSpinRight.DOLocalRotate(new Vector3(0, 0, -360), 2, RotateMode.LocalAxisAdd).SetLoops(-1).SetEase(Ease.Linear).SetId(gameObject.GetInstanceID());
    }

    private void OnDisable()
    {
        DOTween.Kill(gameObject.GetInstanceID());
    }
}
