using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public static class DOTweenUtils
{
    /// <summary> Run the Action<float> from startFloat to endFloat </summary>
    public static Tween DORunFloat(Action<float> actionFloat, float startFloat, float endFloat, float time)
    {
        return DOTween.To(() => startFloat, (value) => actionFloat(value), endFloat, time);
    }
    public static Tween DORunInt(Action<int> actionInt, int startInt, int endInt, float time)
    {
        return DOTween.To(() => startInt, (value) => actionInt(value), endInt, time);
    }

    /// <summary> Try killing the tween </summary>
    public static void TryKill(this Tween tween)
    {
        if (tween != null && tween.IsActive()) { tween.Kill(); }
    }

}
