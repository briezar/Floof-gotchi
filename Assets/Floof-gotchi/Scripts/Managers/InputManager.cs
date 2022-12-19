using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingletonMonoBehaviour<InputManager>
{
    private static Dictionary<KeyCode, Action> Keybinds => Instance._keybinds;
    private Dictionary<KeyCode, Action> _keybinds = new();

    private void Awake()
    {
        enabled = false;
    }

    private void Update()
    {
        foreach (var keybind in Keybinds)
        {
            if (Input.GetKeyDown(keybind.Key))
            {
                keybind.Value?.Invoke();
            }
        }
    }

    public static void SubscribeInput(KeyCode key, Action action, KeyCode secondaryKey = KeyCode.None, bool multipleAction = false)
    {
        if (action == null)
        {
            Instance.CheckEnable();
            return;
        }

        if (secondaryKey != KeyCode.None)
        {
            action = () => { if (Input.GetKey(secondaryKey)) { action(); } };
        }

        if (Keybinds.ContainsKey(key))
        {
            if (multipleAction)
            {
                Keybinds[key] += action;
            }
        }
        else
        {
            Keybinds.Add(key, action);
        }

        Instance.CheckEnable();
    }

    public static void UnsubscribeInput(KeyCode key, Action action)
    {
        Keybinds[key] -= action;
        Instance.CheckEnable();
    }

    public static void ClearKey(KeyCode key)
    {
        Keybinds.Remove(key);
        Instance.CheckEnable();

    }

    private void CheckEnable()
    {
        if (Keybinds.IsNullOrEmpty())
        {
            Instance.enabled = false;
        }
        else
        {
            Instance.enabled = true;
        }
    }

}
