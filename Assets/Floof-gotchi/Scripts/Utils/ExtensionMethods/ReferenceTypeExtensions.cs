using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ReferenceTypeExtensions
{

    #region List

    /// <summary> Remove and return the last element of a list. </summary>
    public static T Pop<T>(this IList<T> list)
    {
        if (list.Count == 0) { return default(T); }
        var lastElem = list[list.Count - 1];
        list.RemoveAt(list.Count - 1);
        return lastElem;
    }

    /// <summary> Get the last element of a list, null if list has 0 elements. </summary>
    public static T GetLast<T>(this IList<T> list)
    {
        var lastElem = list.Count > 0 ? list[list.Count - 1] : default(T);
        return lastElem;
    }

    public static T GetRandom<T>(this IList<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
    {
        return collection == null || collection.Count == 0;
    }

    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
    {
        if (enumerable == null) { return true; }
        var count = 0;
        foreach (var item in enumerable)
        {
            count++;
        }
        return count == 0;
    }


    #endregion

    public static List<T> ToList<T>(this IEnumerable<T> enumerable)
    {
        var list = new List<T>(enumerable);
        return list;
    }

    public static T GetRandom<T>(this T[] array)
    {
        return array[Random.Range(0, array.Length)];
    }
}
