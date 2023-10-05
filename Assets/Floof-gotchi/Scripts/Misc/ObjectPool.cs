using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private T _sample;
    private Stack<T> _poolAvailable = new();
    private Stack<T> _poolGet = new();
    private Transform _parent;

    public Action<T> OnInstantiate;

    public ObjectPool(T element)
    {
        _sample = element;
        _parent = _sample.transform.parent;

        _sample.gameObject.SetActive(false);
    }

    public ObjectPool(T element, Transform parent)
    {
        _sample = element;
        _parent = parent;

        _sample.gameObject.SetActive(false);
    }

    public List<T> AllElements
    {
        get
        {
            var elements = new List<T>();
            elements.AddRange(_poolAvailable);
            elements.AddRange(_poolGet);
            return elements;
        }
    }

    public IReadOnlyCollection<T> ActiveElements => _poolGet;

    public T Get()
    {
        if (!_poolAvailable.TryPop(out var element))
        {
            element = GameObject.Instantiate(_sample, _parent);
            OnInstantiate?.Invoke(element);
        }

        element.gameObject.SetActive(true);
        _poolGet.Push(element);
        return element;

    }

    public void Store(T element)
    {
        if (element == _sample) { return; }

        element.gameObject.SetActive(false);
        element.transform.rotation = Quaternion.identity;
        element.transform.localScale = _sample.transform.localScale;
        element.transform.SetParent(_parent);
        _poolAvailable.Push(element);
        _poolGet.Pop();
    }

    public void StoreAll()
    {
        if (_poolGet.IsNullOrEmpty()) { return; }
        var poolCopy = _poolGet.ToArray();
        foreach (var element in poolCopy)
        {
            Store(element);
        }
    }

    public void Clear(bool destroyElements = false)
    {
        if (destroyElements)
        {
            foreach (var element in _poolAvailable)
            {
                GameObject.Destroy(element.gameObject);
            }
            foreach (var element in _poolGet)
            {
                GameObject.Destroy(element.gameObject);
            }
        }

        _poolAvailable.Clear();
        _poolGet.Clear();
    }
}