using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public static class DOTweenUtils
{
    /// <summary> Run the Action<float> from startFloat to endFloat </summary>
    public static Tween DORunFloat(Action<float> actionFloat, float startFloat, float endFloat, float time, Ease ease = Ease.OutQuad)
    {
        return DOTween.To(() => startFloat, (x) => actionFloat(x), endFloat, time).SetEase(ease);
    }
    public static Tween DORunInt(Action<int> actionInt, int startInt, int endInt, float time, Ease ease = Ease.OutQuad)
    {
        return DOTween.To(() => startInt, (x) => actionInt(x), endInt, time).SetEase(ease);
    }

    /// <summary> Try killing the tween </summary>
    public static void TryKill(this Tween tween)
    {
        if (tween != null && tween.IsActive()) { tween.Kill(); }
    }

}
