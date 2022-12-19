using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public abstract class BaseUI : MonoBehaviour
{
    protected RectTransform rectTransform { get; private set; }

    public int UILayer { get; private set; }
    public bool IsActive => gameObject.activeInHierarchy;
    public bool HasStarted { get; private set; }

    private void Awake()
    {
        rectTransform = (RectTransform)transform;
        OnInit();
    }

    public virtual void OnInit() { }
    public virtual void OnShow() { }
    public virtual void OnHide() { }
    public virtual void OnBack() { }
}
