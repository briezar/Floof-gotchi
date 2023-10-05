using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public static class GeneralUtils
{
    public static Vector3 GetCenterPosition(IEnumerable<Vector3> enumerable)
    {
        var pos = Vector3.zero;
        var count = 0;

        foreach (var item in enumerable)
        {
            pos += item;
            count++;
        }

        return pos / count;
    }

    public static string GenerateRandomName(string prefix = "Player", string randomCharacters = "0123456789", int randomLength = 5)
    {
        var charArray = randomCharacters.ToCharArray();

        var stringBuilder = new StringBuilder();
        stringBuilder.Append(prefix);

        for (int i = 0; i < randomLength; i++)
        {
            var randomChar = charArray.GetRandom();
            stringBuilder.Append(randomChar);
        }

        return stringBuilder.ToString();
    }

    public static string GetDeviceId()
    {
        var deviceId = string.Empty;

        switch (DeviceInfo.CurrentPlatform)
        {
            case Platform.IOS:
                deviceId = SystemInfo.deviceUniqueIdentifier;
                break;

            case Platform.Android:
                var up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                var currentActivity = up.GetStatic<AndroidJavaObject>("currentActivity");
                var contentResolver = currentActivity.Call<AndroidJavaObject>("getContentResolver");
                var secure = new AndroidJavaObject("android.provider.Settings$Secure");
                deviceId = secure.CallStatic<string>("getString", contentResolver, "android_id");
                break;

            default:
                deviceId = SystemInfo.deviceModel;
                break;
        }

        return deviceId;
    }

    public static string GetIDFA()
    {
        switch (DeviceInfo.CurrentPlatform)
        {
            case Platform.IOS:
                return Md5Sum(UnityEngine.iOS.Device.advertisingIdentifier).ToUpper();

            case Platform.Android:
                return Md5Sum(GetDeviceId()).ToUpper();
        }
        return "Unable to get IDFA";
    }

    private static string Md5Sum(string strToEncrypt)
    {
        var encoding = new UTF8Encoding();
        var bytes = encoding.GetBytes(strToEncrypt);

        var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        var hashBytes = md5.ComputeHash(bytes);

        var stringBuilder = new StringBuilder();
        for (int i = 0; i < hashBytes.Length; i++)
        {
            var hash = System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
            stringBuilder.Append(hash);
        }

        var hashString = stringBuilder.ToString();

        return hashString.PadLeft(32, '0');
    }

    public static bool CheckOnScreen(Vector3 pos)
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(pos);
        bool onScreen = screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        return onScreen;
    }

    public static bool ValidatePIN(string PIN)
    {
        if (PIN.IsNullOrEmpty()) { return false; }

        if (PIN.Length != 4 || PIN.Length != 6)
        {
            return false;
        }

        foreach (char c in PIN)
        {
            if (c < '0' || c > '9')
            {
                return false;
            }
        }
        return true;
    }

    public static T[] GetEnums<T>() where T : Enum
    {
        var array = (T[])Enum.GetValues(typeof(T));
        return array;
    }

    public static bool ClickedInsideRect(RectTransform rectTransform, Camera cam = null)
    {
        if (Input.GetMouseButtonDown(0))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, cam ?? Camera.main, out var localPoint);
            return rectTransform.rect.Contains(localPoint);
        }

        return false;
    }

    /// <summary> Get a random index from an array of chances based on its values </summary>
    public static int GetIndexFromLootTable(params int[] chances)
    {
        if (chances.Length == 0)
        {
            Debug.LogError("Loot Table has 0 element!");
            return 0;
        }

        if (chances.Length == 1) { return 0; }

        int tableLength = chances.Length;

        int total = 0;

        for (int i = 0; i < tableLength; i++)
        {
            total += chances[i];
        }

        var randomChance = UnityEngine.Random.Range(0, total + 1);

        for (var i = 0; i < tableLength; i++)
        {
            if (chances[i] <= 0) { continue; }
            if (randomChance <= chances[i])
            {
                return i;
            }
            else
            {
                randomChance -= chances[i];
            }
        }
        Debug.LogWarning("Exit Loot Table");
        return 0;
    }

    public static string GetMethodCallerInfo(int frameSkip = 0)
    {
        var stackTrace = new System.Diagnostics.StackTrace(true);
        var frame = stackTrace.GetFrame(2 + frameSkip); // 0 is this method, 1 is this method's caller, 2 is the upper method

        var methodName = frame.GetMethod().Name.Replace("b__0", "").Replace("<", "").Replace(">", "");
        var fileName = System.IO.Path.GetFileName(frame.GetFileName());
        var lineNumber = frame.GetFileLineNumber().ToString();
        var output = $"{methodName.Colorize(Color.yellow)} in {fileName.Colorize(Color.yellow)} at line {lineNumber.Colorize(Color.yellow)}";

        return output;
    }

}