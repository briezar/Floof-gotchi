
using UnityEngine;
using static RectTransformPreset;

// Adapted from https://answers.unity.com/questions/1225118/solution-set-ui-recttransform-anchor-presets-from.html
public class RectTransformPreset
{
    public static readonly float[] LCR = new float[] { 0, 0.5f, 1 };
    public static readonly float[] TMB = new float[] { 1, 0.5f, 0 };

    public enum Anchor
    {
        TopLeft, TopCenter, TopRight,
        MiddleLeft, MiddleCenter, MiddleRight,
        BottomLeft, BottomCenter, BottomRight,
        V_StretchLeft, V_StretchRight, V_StretchCenter,
        H_StretchTop, H_StretchMiddle, H_StretchBottom,
        StretchAll
    }

    public enum Pivot
    {
        TopLeft, TopCenter, TopRight,
        MiddleLeft, MiddleCenter, MiddleRight,
        BottomLeft, BottomCenter, BottomRight
    }

}

public static class RectTransformExtensions
{
    public static void SetAnchor(this RectTransform source, Anchor anchor, int offsetX = 0, int offsetY = 0)
    {
        source.anchoredPosition = new Vector2(offsetX, offsetY);

        var x = RectTransformPreset.LCR[((int)anchor) % 3];
        var y = RectTransformPreset.TMB[(int)anchor / 3];

        switch (anchor)
        {
            case (Anchor.TopLeft):
            case (Anchor.TopCenter):
            case (Anchor.TopRight):
            case (Anchor.MiddleLeft):
            case (Anchor.MiddleCenter):
            case (Anchor.MiddleRight):
            case (Anchor.BottomLeft):
            case (Anchor.BottomCenter):
            case (Anchor.BottomRight):
                {
                    source.anchorMin = new Vector2(x, y);
                    source.anchorMax = new Vector2(x, y);
                    break;
                }

            case (Anchor.V_StretchLeft):
            case (Anchor.V_StretchCenter):
            case (Anchor.V_StretchRight):
                {
                    source.anchorMin = new Vector2(x, 0);
                    source.anchorMax = new Vector2(x, 1);
                    break;
                }

            case (Anchor.H_StretchTop):
            case (Anchor.H_StretchMiddle):
            case (Anchor.H_StretchBottom):
                {
                    source.anchorMin = new Vector2(0, y);
                    source.anchorMax = new Vector2(1, y);
                    break;
                }

            case (Anchor.StretchAll):
                {
                    source.anchorMin = new Vector2(0, 0);
                    source.anchorMax = new Vector2(1, 1);
                    break;
                }
        }
    }

    public static void SetPivot(this RectTransform source, Pivot pivot)
    {
        var x = RectTransformPreset.LCR[((int)pivot) % 3];
        var y = RectTransformPreset.TMB[(int)pivot / 3];

        source.pivot = new Vector2(x, y);
    }

    public static void SetAnchorAndPivot(this RectTransform rectTransform, Pivot pivot)
    {
        SetAnchor(rectTransform, (Anchor)pivot);
        SetPivot(rectTransform, pivot);
    }

    public static void SetWidth(this RectTransform rectTransform, float width)
    {
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    }

    public static void SetHeight(this RectTransform rectTransform, float height)
    {
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }
}