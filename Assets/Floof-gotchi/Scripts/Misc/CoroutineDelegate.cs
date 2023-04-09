using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CoroutineDelegate
{
    private MonoBehaviour _coroutineRunner;
    public CoroutineDelegate(MonoBehaviour coroutineRunner)
    {
        _coroutineRunner = coroutineRunner;
    }

    public Coroutine WaitAndDo(float delay, Action callback)
    {
        return StartCoroutine(WaitAndDo());
        IEnumerator WaitAndDo()
        {
            yield return new WaitForSeconds(delay);
            callback?.Invoke();
        }
    }

    public Coroutine StartCoroutine(IEnumerator routine)
    {
        if (routine == null) { return null; }
        return _coroutineRunner.StartCoroutine(routine);
    }

    public void StopCoroutine(Coroutine coroutine)
    {
        if (coroutine != null)
        {
            _coroutineRunner.StopCoroutine(coroutine);
        }
    }
    public void StopCoroutine(string routine)
    {
        if (routine != null)
        {
            _coroutineRunner.StopCoroutine(routine);
        }
    }
}
