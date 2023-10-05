using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceInfo
{
    public static readonly Platform CurrentPlatform =
    (
#if UNITY_EDITOR
        Platform.Editor
#elif UNITY_IOS
        Platform.IOS
#elif UNITY_ANDROID
        Platform.Android
#else
        Platform.Undefined
#endif
    );
}

public enum Platform
{
    IOS,
    Android,
    Editor,
    Undefined
}

public class Internet
{
    private const long Kilobyte = 1024;

    public enum DataUnit { Byte = 0, KB = 1, MB = 2, GB = 3 }

    public static string GetShortenedSize(long dataSizeInBytes)
    {
        // MB = KB^2, GB = KB^3
        var dataUnit = (int)Mathf.Log(dataSizeInBytes, Kilobyte).ClampMin(1);
        var scaledDataUnit = Kilobyte * dataUnit;
        var shortenedSize = (float)dataSizeInBytes / scaledDataUnit;

        return $"{shortenedSize:F2} {(DataUnit)dataUnit}";
    }

    public static bool IsConnected => (Application.internetReachability != NetworkReachability.NotReachable);
}
