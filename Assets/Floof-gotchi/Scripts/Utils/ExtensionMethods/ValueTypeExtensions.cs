using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ValueTypeExtensions
{
    public static bool IsNullOrEmpty(this string text)
    {
        return string.IsNullOrEmpty(text);
    }

    public static string LastChar(this string text)
    {
        return text.IsNullOrEmpty() ? string.Empty : text[text.Length - 1].ToString();
    }


}
