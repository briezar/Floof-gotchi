using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Utils
{
    public static void WaitAndDo(float delay, Action action)
    {
        StartCoroutine(WaitThenDoRoutine());
        IEnumerator WaitThenDoRoutine()
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }
    }

    public static Coroutine StartCoroutine(IEnumerator enumerator)
    {
        return GameManager.Instance.StartCoroutine(enumerator);
    }
    public static void StopCoroutine(IEnumerator enumerator)
    {
        if (enumerator != null) { GameManager.Instance.StopCoroutine(enumerator); }
    }
    public static void StopCoroutine(Coroutine routine)
    {
        if (routine != null) { GameManager.Instance.StopCoroutine(routine); }
    }


    public static Vector3 GetRandomPoint(Vector3 min, Vector3 max)
    {
        return min + Random.Range(0f, 1f) * (max - min);
    }

}


public class PlayerPrefsExt : PlayerPrefs
{
    public static bool GetBool(string key, bool defaultValue = false)
    {
        return GetInt(key, defaultValue ? 1 : 0) == 1;
    }

    public static void SetBool(string key, bool value)
    {
        SetInt(key, value ? 1 : 0);
    }
}
