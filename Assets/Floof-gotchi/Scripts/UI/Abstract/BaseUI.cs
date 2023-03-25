using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUI : MonoBehaviour
{
    private RectTransform _rectTransform;
    public RectTransform rectTransform { get => _rectTransform ?? (_rectTransform = (RectTransform)transform); }

    private CanvasGroup _canvasGroup;
    public CanvasGroup canvasGroup { get => _canvasGroup ?? (_canvasGroup = this.ForceGetComponent<CanvasGroup>()); }

    public bool IsActive => gameObject.activeInHierarchy;

}
