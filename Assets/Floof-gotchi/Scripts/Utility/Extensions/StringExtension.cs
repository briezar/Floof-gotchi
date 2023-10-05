using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public static class StringExtension
{
    public static bool ToBool(this string text)
    {
        text = text?.Trim();
        if (text.IsNullOrEmpty()) { return default; }
        if (!bool.TryParse(text, out var result))
        {
            Debug.LogWarning("Invalid string to bool format: " + text);
        }
        return result;
    }
    public static int ToInt(this string text)
    {
        text = text?.Trim();
        if (text.IsNullOrEmpty()) { return default; }
        if (!int.TryParse(text, out var result))
        {
            Debug.LogWarning("Invalid string to int format: " + text);
        }
        return result;
    }
    public static float ToFloat(this string text)
    {
        text = text?.Trim();
        if (text.IsNullOrEmpty()) { return default; }
        if (!float.TryParse(text, out var result))
        {
            Debug.LogWarning("Invalid string to float format: " + text);
        }
        return result;
    }
    public static long ToLong(this string text)
    {
        text = text?.Trim();
        if (text.IsNullOrEmpty()) { return default; }
        if (!long.TryParse(text, out var result))
        {
            Debug.LogWarning("Invalid string to long format: " + text);
        }
        return result;
    }
    public static T ToEnum<T>(this string text, bool ignoreCase = true) where T : struct, IConvertible
    {
        if (text == null) { return default; }
        text = text.Trim();
        if (text.IsNullOrEmpty()) { return default; }
        if (!Enum.TryParse<T>(text, ignoreCase, out T result))
        {
            Debug.LogWarning($"Cannot parse string to {typeof(T)}: {text}");
        }
        return result;
    }

    public static List<int> ToIntList(this string text, string separator = ",")
    {
        var splitString = text.Split(separator);
        var list = new List<int>();

        foreach (var value in splitString)
        {
            list.Add(value.ToInt());
        }

        return list;
    }

    public static bool IsNullOrEmpty(this string text)
    {
        return string.IsNullOrEmpty(text);
    }

    public static string Format(this string text, params object[] args)
    {
        return string.Format(text, args);
    }

    public static string CleanInput(this string input)
    {
        // Replace invalid characters with empty strings.
        try
        {
            return Regex.Replace(input, @"\p{C}+", "", RegexOptions.None, TimeSpan.FromSeconds(1.5));
        }
        // If timeout when replacing invalid characters, return Empty.
        catch (RegexMatchTimeoutException)
        {
            return String.Empty;
        }
    }

    public static string Reverse(this string input)
    {
        var reversedString = string.Create<string>(input.Length, input, (chars, state) =>
        {
            state.AsSpan().CopyTo(chars);
            chars.Reverse();
        });
        return reversedString;
    }

    public static string ToHex(this string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        var hexString = BitConverter.ToString(bytes).Replace("-", "");
        return hexString;
    }

    public static string FromHex(this string hexString)
    {
        var bytes = new byte[hexString.Length / 2];
        for (var i = 0; i < bytes.Length; i++)
        {
            bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
        }

        return Encoding.Unicode.GetString(bytes);
    }

    public static string LastChar(this string text)
    {
        return text[text.Length - 1].ToString();
    }

    public static string GetBetween(this string text, int startIndex, int endIndex)
    {
        if (startIndex > text.Length || endIndex > text.Length) { Debug.LogWarning($"Index exceeds string length!"); }
        return text.Substring(startIndex, endIndex - startIndex);
    }

    /// <summary> "SeparateCamelCase" -> "Separate Camel Case" </summary>
    public static string SeparateCamelCase(this string text, string separator = " ")
    {
        return Regex.Replace(text, @"(\p{Lu})(?<=\p{Ll}\1|(\p{Lu}|\p{Ll})\1(?=\p{Ll}))", separator + "$1");
    }

    public static string Bold(this string text)
    {
        return $"<b>{text}</b>";
    }
    public static string Colorize(this string text, Color color)
    {
        var hex = ColorUtility.ToHtmlStringRGBA(color);
        return $"<color=#{hex}>{text}</color>";
    }
    public static string Colorize(this string text, string hex)
    {
        return $"<color=#{hex}>{text}</color>";
    }
    public static string Resize(this string text, float scale)
    {
        return $"<size={scale * 100}%>{text}</size>";
    }
    public static string NoBreak(this string text)
    {
        return $"<nobr>{text}</nobr>";
    }

}
