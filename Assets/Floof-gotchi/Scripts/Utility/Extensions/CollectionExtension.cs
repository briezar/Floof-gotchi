using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class CollectionExtension
{
    public static List<T> ToList<T>(this IEnumerable<T> enumerable)
    {
        return new List<T>(enumerable);
    }

    /// <summary> Remove and return the last element of a list. </summary>
    public static T Pop<T>(this List<T> list)
    {
        if (list.IsNullOrEmpty()) { return default; }
        var lastElem = list[list.Count - 1];
        list.RemoveAt(list.Count - 1);
        return lastElem;
    }

    /// <summary> Add to the start of a list. </summary>
    public static void Enqueue<T>(this List<T> list, T element)
    {
        if (list == null)
        {
            Debug.LogError($"List is null");
            return;
        }
        list.Insert(0, element);
    }

    /// <summary> Remove and return the first element of a list. </summary>
    public static T Dequeue<T>(this List<T> list)
    {
        if (list.IsNullOrEmpty()) { return default; }
        var firstElem = list[0];
        list.RemoveAt(0);
        return firstElem;
    }

    /// <summary> Get the first element of a list, null if list has 0 elements. </summary>
    public static T GetFirst<T>(this IList<T> list)
    {
        T firstElem = list.IsNullOrEmpty() ? default(T) : list[0];
        return firstElem;
    }
    /// <summary> Get the last element of a list, null if list has 0 elements. </summary>
    public static T GetLast<T>(this IList<T> list)
    {
        T lastElem = list.IsNullOrEmpty() ? default(T) : list[list.Count - 1];
        return lastElem;
    }

    public static bool Contains<T>(this T[] array, T element)
    {
        return Array.Exists(array, (match) => match.Equals(element));
    }
    public static bool Exists<T>(this T[] array, Predicate<T> match)
    {
        return Array.Exists(array, match);
    }
    public static T Find<T>(this T[] array, Predicate<T> match)
    {
        return Array.Find(array, match);
    }
    public static T[] FindAll<T>(this T[] array, Predicate<T> match)
    {
        return Array.FindAll(array, match);
    }

    public static T DequeueWithDefault<T>(this Queue<T> queue, T defaultValue)
    {
        if (queue == null)
        {
            return defaultValue;
        }

        if (queue.Count > 0)
        {
            return queue.Dequeue();
        }

        return defaultValue;
    }

    public static string JoinElements<T>(this IEnumerable<T> enumerable, string separator = ", ")
    {
        return string.Join(separator, enumerable);
    }
    public static string JoinElements<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, string format = "{0}:{1}", string separator = "\n")
    {
        var result = new List<string>();
        foreach (var item in dictionary)
        {
            result.Add(string.Format(format, item.Key, item.Value));
        }
        return result.JoinElements(separator);
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
        return enumerable == null || count == 0;
    }

    public static int Sum(this ICollection<int> collection)
    {
        int sum = 0;
        foreach (var value in collection)
        {
            sum += value;
        }
        return sum;
    }

    public static int Max(this IEnumerable<int> enumerable)
    {
        int max = 0;
        foreach (var value in enumerable)
        {
            if (value > max) { max = value; }
        }
        return max;
    }

    public static T[] ToArray<T>(this HashSet<T> set)
    {
        T[] array = new T[set.Count];
        set.CopyTo(array);
        return array;
    }

    public static HashSet<T> ToHashSet<T>(this IEnumerable<T> enumerable)
    {
        return new HashSet<T>(enumerable);
    }

    public static T GetRandom<T>(this IList<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    public static T GetRandom<T>(this ICollection<T> collection)
    {
        var randomIndex = Random.Range(0, collection.Count);
        var counter = 0;
        foreach (var item in collection)
        {
            if (counter == randomIndex)
            {
                return item;
            }
            counter++;
        }
        return default(T);
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = UnityEngine.Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    public static List<T> ToListShuffled<T>(this IEnumerable<T> enumerable)
    {
        var list = enumerable.ToList();
        list.Shuffle();
        return list;
    }
    public static void ForEach<T>(this IEnumerable<T> sequence, Action<int, T> action)
    {
        // argument null checking omitted
        int i = 0;
        foreach (T item in sequence)
        {
            action(i, item);
            i++;
        }
    }

}
