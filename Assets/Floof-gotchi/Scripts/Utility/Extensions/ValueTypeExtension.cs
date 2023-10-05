using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ValueTypeExtension
{
    public static int Clamp(this int value, int min, int max)
    {
        return Mathf.Clamp(value, min, max);
    }
    public static int ClampMin(this int value, int min)
    {
        return Mathf.Clamp(value, min, value);
    }
    public static int ClampMax(this int value, int max)
    {
        return Mathf.Clamp(value, value, max);
    }
    public static int ClampCollection(this int value, ICollection collection)
    {
        return Mathf.Clamp(value, 0, collection.Count - 1);
    }


    public static float Round(this float value, int decimalPlace)
    {
        // 0.1223 , 2 decimal place
        // 12.23
        // 12
        // 0.12
        var multiply = Mathf.Pow(10, decimalPlace);
        var roundedValue = Mathf.Round(value * multiply);
        return roundedValue / multiply;
    }

    public static Color Transparent(this Color color, float alpha = 0)
    {
        color.a = alpha;
        return color;
    }

    public static int DaysFrom(this DateTime thisDate, DateTime otherDate)
    {
        var days = (thisDate - otherDate).Days;
        return Mathf.Abs(days);
    }


    public static float Clamp(this float value, float min, float max)
    {
        return Mathf.Clamp(value, min, max);
    }
    public static float ClampMin(this float value, float min)
    {
        return Mathf.Clamp(value, min, value);
    }

    public static Vector3 Shift(this Vector3 vector, float x, float y = 0, float z = 0)
    {
        return vector + new Vector3(x, y, z);
    }

    public static Vector2 Shift(this Vector2 vector, float x, float y = 0)
    {
        return vector + new Vector2(x, y);
    }
}
