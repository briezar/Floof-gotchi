using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class YieldCollection
{
    public const float Epsilon = 0.0001f;
    private static readonly WaitForEndOfFrame _waitForEndOfFrame = new();
    private static readonly WaitForFixedUpdate _waitForFixedUpdate = new();

    // Adapted from https://forum.unity.com/threads/c-coroutine-waitforseconds-garbage-collection-tip.224878/#post-2436633
    private class FloatComparer : IEqualityComparer<float>
    {
        bool IEqualityComparer<float>.Equals(float x, float y)
        {
            return Mathf.Abs(x - y) <= Epsilon;
        }
        int IEqualityComparer<float>.GetHashCode(float obj)
        {
            return obj.GetHashCode();
        }
    }

    private static Dictionary<float, WaitForSeconds> _yieldInstructionDict = new(100, new FloatComparer());

    public static WaitForSeconds WaitForSeconds(float duration)
    {
        if (duration < (1f / Application.targetFrameRate)) { return null; }

        if (!_yieldInstructionDict.TryGetValue(duration, out var waitYield))
        {
            _yieldInstructionDict.Add(duration, waitYield = new WaitForSeconds(duration));
            // Debug.LogWarning($"New yield added: {duration:F5}, Count: {_yieldInstructionDict.Count}");
        }

        return waitYield;
    }

    public static WaitForEndOfFrame WaitForEndOfFrame()
    {
        return _waitForEndOfFrame;
    }

    public static WaitForFixedUpdate WaitForFixedUpdate()
    {
        return _waitForFixedUpdate;
    }

    public static IEnumerator WaitUntil(Func<bool> condition)
    {
        if (condition == null || condition()) { yield break; }
        while (!condition())
        {
            yield return null;
        }
    }

    public static IEnumerator WaitForFrames(int frameCount)
    {
        if (frameCount <= 0)
        {
            Debug.LogWarning("Cannot wait for less that 1 frame");
            yield break;
        }

        while (frameCount > 0)
        {
            frameCount--;
            yield return null;
        }
    }
}
