using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    #region String
    public static bool IsNullOrEmpty(this string text)
    {
        return string.IsNullOrEmpty(text);
    }

    public static string LastChar(this string text)
    {
        return text[text.Length - 1].ToString();
    }

    #endregion


    #region List

    /// <summary> Remove and return the last element of a list. </summary>
    public static T Pop<T>(this List<T> list)
    {
        if (list.Count == 0) { return default(T); }
        var lastElem = list[list.Count - 1];
        list.RemoveAt(list.Count - 1);
        return lastElem;
    }

    /// <summary> Get the last element of a list, null if list has 0 elements. </summary>
    public static T GetLast<T>(this List<T> list)
    {
        var lastElem = list.Count > 0 ? list[list.Count - 1] : default(T);
        return lastElem;
    }

    public static T GetRandom<T>(this List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
    {
        return collection == null || collection.Count == 0;
    }


    #endregion


    #region Array

    public static List<T> ToList<T>(T[] array)
    {
        var list = new List<T>();
        list.AddRange(array);
        return list;
    }

    public static T GetRandom<T>(this T[] array)
    {
        return array[Random.Range(0, array.Length)];
    }

    #endregion

}
