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


    #region GameObject
    public static void DestroyGameObject(this Component component, bool releaseMemory = false)
    {
        if (releaseMemory)
        {
            UnityEngine.AddressableAssets.Addressables.ReleaseInstance(component.gameObject);
        }
        else
        {
            GameObject.Destroy(component.gameObject);
        }
    }

    #endregion


    public static T ForceGetComponent<T>(this Component component) where T : Component
    {
        if (!component.TryGetComponent<T>(out var result))
        {
            result = component.gameObject.AddComponent<T>();
        }
        return result;
    }

    public static Vector3[] GetWorldCorners(this RectTransform rectTransform)
    {
        var worldCorners = new Vector3[4];
        rectTransform.GetWorldCorners(worldCorners);
        return worldCorners;
    }

    public static float GetAspectRatio(this Sprite sprite)
    {
        return sprite.rect.width / sprite.rect.height;
    }
}

public static class RectTransformExtensions
{
    public static void HeightControlsWidth(this RectTransform rectTransform, float aspectRatio)
    {
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectTransform.rect.height * aspectRatio);
    }

    public static void WidthControlsHeight(this RectTransform rectTransform, float aspectRatio)
    {
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rectTransform.rect.width / aspectRatio);
    }

    public static void FitInParent(this RectTransform rectTransform, float aspectRatio, bool envelopeParent = false)
    {
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.anchoredPosition = Vector2.zero;

        Vector2 sizeDelta = Vector2.zero;
        Vector2 parentSize = rectTransform.GetParentSize();
        if ((parentSize.y * aspectRatio < parentSize.x) ^ (!envelopeParent))
        {
            sizeDelta.y = rectTransform.GetSizeDeltaToProduceSize(parentSize.x / aspectRatio, 1);
        }
        else
        {
            sizeDelta.x = rectTransform.GetSizeDeltaToProduceSize(parentSize.y * aspectRatio, 0);
        }
        rectTransform.sizeDelta = sizeDelta;
    }

    public static Vector2 GetParentSize(this RectTransform rectTransform)
    {
        RectTransform parent = rectTransform.parent as RectTransform;
        if (!parent)
        {
            return Vector2.zero;
        }
        return parent.rect.size;
    }

    public static float GetSizeDeltaToProduceSize(this RectTransform rectTransform, float size, int axis)
    {
        return size - rectTransform.GetParentSize()[axis] * (rectTransform.anchorMax[axis] - rectTransform.anchorMin[axis]);
    }

}