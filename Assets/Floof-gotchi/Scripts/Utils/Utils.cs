using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static void WaitThenDo(float delay, Action action)
    {
        GameManager.Instance.StartCoroutine(WaitThenDoRoutine());
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

    public static void DestroyGameObject(this Component component)
    {
        if (!UnityEngine.AddressableAssets.Addressables.ReleaseInstance(component.gameObject))
        {
            GameObject.Destroy(component.gameObject);
        }
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
