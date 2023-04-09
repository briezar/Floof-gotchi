using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class ExtensionMethods
{
    public static bool IsMethodOverridden<T>(this T _class, string methodName)
    {
        var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;
        return _class.GetType().GetMember(methodName, bindingFlags).Length == 0;
    }
}