using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public abstract class BaseUI : MonoBehaviour
{
    public RectTransform rectTransform { get; private set; }
    public CanvasGroup canvasGroup { get; private set; }

    public UILayer Layer { get; private set; }
    public bool IsActive => gameObject.activeInHierarchy;

    private void Awake()
    {
        rectTransform = (RectTransform)transform;
        canvasGroup = this.ForceGetComponent<CanvasGroup>();
        OnInit();
    }

    public void SetLayer(UILayer layer)
    {
        Layer = layer;
    }

    public virtual void OnInit() { }
    public virtual void OnShow() { }
    public virtual void OnHide() { }
    public virtual void OnBack() { }
}
