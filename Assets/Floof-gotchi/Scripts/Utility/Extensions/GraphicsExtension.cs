using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class GraphicsExtension
{
    public static void SetOnClick(this Button button, Action action)
    {
        button.onClick = new(); // remove events added through inspector
        button.onClick.AddListener(() => action?.Invoke());
    }

    public static void SetOnValueChanged(this Toggle toggle, Action<bool> action)
    {
        toggle.onValueChanged = new(); // remove events added through inspector
        toggle.onValueChanged.AddListener((isOn) => action?.Invoke(isOn));
    }

    public static T SetAlpha<T>(this T graphic, float newAlpha) where T : Graphic
    {
        var color = graphic.color;
        color.a = newAlpha;
        graphic.color = color;
        return graphic;
    }

    public static void SetAlpha(this SpriteRenderer renderer, float newAlpha)
    {
        var color = renderer.color;
        color.a = newAlpha;
        renderer.color = color;
    }

    public static Color GetTransparentColor(this SpriteRenderer renderer, float alpha = 0)
    {
        var color = renderer.color;
        color.a = alpha;
        return color;
    }
}
