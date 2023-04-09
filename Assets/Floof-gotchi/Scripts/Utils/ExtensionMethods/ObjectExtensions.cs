using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObjectExtensions
{
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

    public static T ForceGetComponent<T>(this Component component) where T : Component
    {
        if (!component.TryGetComponent<T>(out var result))
        {
            result = component.gameObject.AddComponent<T>();
        }
        return result;
    }
}

public static class RectTransformExtensions
{

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