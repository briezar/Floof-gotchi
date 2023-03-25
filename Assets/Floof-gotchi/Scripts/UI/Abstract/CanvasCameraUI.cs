using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public abstract class CanvasCameraUI : BaseUI
{
    public UILayer Layer { get; private set; }

    private void Awake()
    {
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
