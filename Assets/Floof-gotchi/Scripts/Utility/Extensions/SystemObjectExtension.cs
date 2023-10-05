using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SystemObjectExtension
{
    public static string GetClassInfo(this object obj, string separator = ", ")
    {
        var textList = new List<string>();
        var type = obj.GetType();
        textList.Add($" [{type.Name.Colorize(Color.green)}] ");
        var fields = type.GetFields();
        foreach (var field in fields)
        {
            textList.Add($"{field.Name.Colorize(Color.yellow)}: {field.GetValue(obj)}");
        }
        return textList.JoinElements(separator);
    }
}
