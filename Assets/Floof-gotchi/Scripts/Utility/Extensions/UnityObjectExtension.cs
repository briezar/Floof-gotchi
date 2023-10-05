using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SpecializedExtension
{
    public static class SpecializedObjectExtension
    {
        // public static void SetUISortingLayer(this GameObject gameObject, bool allowClick = false, string layer = Constants.SortingLayerName.OnTop, int sortingOrder = 0)
        // {
        //     if (gameObject == null) { throw new NullReferenceException($"{gameObject.name} is null or destroyed!"); }

        //     bool alreadyActive = gameObject.activeSelf;

        //     // GameObject must be active to change sorting
        //     gameObject.SetActive(true);

        //     var canvas = gameObject.ForceGetComponent<Canvas>();

        //     if (gameObject.activeInHierarchy)
        //     {
        //         OverrideSorting();
        //     }
        //     else
        //     {
        //         gameObject.AddComponent<GameObjectLifeCycleDelegate>().Enabled = OverrideSorting;
        //     }

        //     if (allowClick) { gameObject.ForceGetComponent<GraphicRaycaster>(); }

        //     if (!alreadyActive) { gameObject.SetActive(false); }


        //     void OverrideSorting()
        //     {
        //         if (canvas == null) { return; }

        //         canvas.overrideSorting = true;
        //         canvas.sortingOrder = sortingOrder;

        //         if (!layer.IsNullOrEmpty())
        //         {
        //             canvas.sortingLayerName = layer;
        //         }
        //     }
        // }
        // public static void SetUISortingLayer(this Component component, bool allowClick = false, string layer = Constants.SortingLayerName.OnTop, int sortingOrder = 0)
        // {
        //     SetUISortingLayer(component.gameObject, allowClick, layer, sortingOrder);
        // }

        public static void StopOverrideSorting(this GameObject gameObject, bool destroyCanvas = false)
        {
            if (gameObject == null) { throw new NullReferenceException($"{gameObject.name} is null or destroyed!"); }

            if (gameObject.TryGetComponent<Canvas>(out var canvas))
            {
                if (!destroyCanvas)
                {
                    canvas.overrideSorting = false;
                    return;
                }
                if (gameObject.TryGetComponent<GraphicRaycaster>(out var raycaster)) { GameObject.Destroy(raycaster); }
                GameObject.Destroy(canvas);
                Canvas.ForceUpdateCanvases();
            }
        }
        public static void StopOverrideSorting(this Component component, bool destroyCanvas = false)
        {
            StopOverrideSorting(component.gameObject, destroyCanvas);
        }


        public static void AddEventTrigger(this GameObject gameObject, Action action, EventTriggerType type = EventTriggerType.PointerDown)
        {
            if (gameObject == null) { throw new NullReferenceException($"{gameObject.name} is null or destroyed!"); }

            var trigger = gameObject.ForceGetComponent<EventTrigger>();
            var entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((eventData) => { action?.Invoke(); });
            trigger.triggers.Add(entry);
        }
        public static void AddEventTrigger(this Component component, Action action, EventTriggerType type = EventTriggerType.PointerDown)
        {
            AddEventTrigger(component.gameObject, action, type);
        }


        /// <summary> Scales with transform! </summary>
        public static Vector3 GetMiddleOfEdge(this GameObject gameObject, RectTransform.Edge edge, float xDiff = 0f, float yDiff = 0f)
        {
            if (gameObject == null) { throw new NullReferenceException($"{gameObject.name} is null or destroyed!"); }

            var rectTransform = gameObject.transform as RectTransform;
            if (rectTransform == null)
            {
                Debug.LogWarning($"{gameObject} does not have RectTransform component");
                return Vector3.zero;
            }

            var height = rectTransform.rect.height;
            var width = rectTransform.rect.width;
            var localPoint = Vector3.zero;

            switch (edge)
            {
                case RectTransform.Edge.Left:
                    localPoint.x -= width / 2;
                    break;
                case RectTransform.Edge.Right:
                    localPoint.x += width / 2;
                    break;
                case RectTransform.Edge.Top:
                    localPoint.y += height / 2;
                    break;
                case RectTransform.Edge.Bottom:
                    localPoint.y -= height / 2;
                    break;
            }

            var worldPoint = rectTransform.TransformPoint(localPoint);
            worldPoint.x += xDiff;
            worldPoint.y += yDiff;
            return worldPoint;
        }
        public static Vector3 GetMiddleOfEdge(this Component component, RectTransform.Edge edge, float xDiff = 0f, float yDiff = 0f)
        {
            return GetMiddleOfEdge(component.gameObject, edge, xDiff, yDiff);
        }

    }

}
public static class ObjectExtension
{
    public static bool IsUnityComponent(this Type type)
    {
        return typeof(Component).IsAssignableFrom(type);
    }

    public static GameObject GetParent(this GameObject gameObject)
    {
        return gameObject.transform.parent.gameObject;
    }
    public static GameObject GetParent(this Component component)
    {
        return GetParent(component.gameObject);
    }

    public static void DestroyGameObject(this GameObject gameObject)
    {
        if (gameObject == null) { return; }
        GameObject.Destroy(gameObject);
        gameObject = null;
    }
    public static void DestroyGameObject(this Component component)
    {
        DestroyGameObject(component.gameObject);
    }

    public static T ForceGetComponent<T>(this GameObject gameObject) where T : Component
    {
        if (gameObject == null) { return null; }

        if (!gameObject.TryGetComponent<T>(out var result))
        {
            result = gameObject.AddComponent<T>();
        }
        return result;
    }
    public static T ForceGetComponent<T>(this Component component) where T : Component
    {
        return ForceGetComponent<T>(component.gameObject);
    }
    public static T AddComponent<T>(this Component component) where T : Component
    {
        var newComponent = component.gameObject.AddComponent<T>();
        return newComponent;
    }

    public static RectTransform GetRectTransform(this Component component)
    {
        return component.transform as RectTransform;
    }
    public static RectTransform GetRectTransform(this GameObject gameObject)
    {
        return gameObject.transform as RectTransform;
    }

    public static void DestroyAllChildren(this Transform parent)
    {
        foreach (Transform child in parent)
        {
            child.DestroyGameObject();
        }
    }

    public static List<Transform> GetChildList(this Transform parent, bool getInactive = true)
    {
        var childList = new List<Transform>();
        foreach (Transform child in parent)
        {
            if (!getInactive && !child.gameObject.activeInHierarchy) { continue; }

            childList.Add(child);
        }

        return childList;
    }

    /// <summary> 0 = BottomLeft, 1 = TopLeft, 2 = TopRight, 3 = BottomRight </summary>
    public static Vector3[] GetWorldCorners(this Transform transform)
    {
        var rectTransform = transform as RectTransform;
        if (rectTransform == null) { return null; }

        var worldCorners = new Vector3[4];
        rectTransform.GetWorldCorners(worldCorners);

        return worldCorners;
    }

    public static void ClampInScreen(this Transform transform, float offset = 0, Camera camera = default)
    {
        camera ??= Camera.main;

        var safeArea = Screen.safeArea;
        var minPos = camera.ScreenToWorldPoint(safeArea.min);
        var maxPos = camera.ScreenToWorldPoint(safeArea.max);

        var corners = transform.GetWorldCorners();
        var minCorner = corners[0];
        var maxCorner = corners[2];

        float xOffset = 0;
        float yOffset = 0;

        if (minCorner.x < minPos.x)
        {
            xOffset = minPos.x - minCorner.x;
        }
        else if (maxCorner.x > maxPos.x)
        {
            xOffset = maxPos.x - maxCorner.x;
        }

        xOffset += offset * Mathf.Sign(xOffset);

        if (minCorner.y < minPos.y)
        {
            yOffset = minPos.y - minCorner.y;
        }
        else if (maxCorner.y > maxPos.y)
        {
            yOffset = maxPos.y - maxCorner.y;
        }

        yOffset += offset * Mathf.Sign(yOffset);

        transform.position += new Vector3(xOffset, yOffset);
    }

    public static Coroutine WaitAndDo(this MonoBehaviour monoBehaviour, float delay, Action action)
    {
        return monoBehaviour.StartCoroutine(WaitAndDoRoutine());
        IEnumerator WaitAndDoRoutine()
        {
            yield return YieldCollection.WaitForSeconds(delay);
            action?.Invoke();
        }
    }

    // public static Coroutine WaitForMouseClick(this MonoBehaviour monoBehaviour, Action action, bool disableInteraction = true)
    // {
    //     return monoBehaviour.StartCoroutine(WaitRoutine());

    //     IEnumerator WaitRoutine()
    //     {
    //         if (disableInteraction) { ViewManager.SetInteractable(false); }
    //         while (true)
    //         {
    //             if (Input.GetMouseButtonDown(0))
    //             {
    //                 action?.Invoke();
    //                 if (disableInteraction) { ViewManager.SetInteractable(true); }
    //                 break;
    //             }
    //             yield return null;
    //         }
    //     }
    // }

    public static void RebuildImmediate(this LayoutGroup layout)
    {
        layout.CalculateLayoutInputVertical();
        layout.CalculateLayoutInputHorizontal();
        layout.SetLayoutVertical();
        layout.SetLayoutHorizontal();
    }

    public static T Clone<T>(this T scriptableObject) where T : ScriptableObject
    {
        if (scriptableObject == null)
        {
            Debug.LogError($"ScriptableObject was null. Returning default {typeof(T)} object.");
            return (T)ScriptableObject.CreateInstance(typeof(T));
        }

        T instance = UnityEngine.Object.Instantiate(scriptableObject);
        instance.name = scriptableObject.name; // remove (Clone) from name
        return instance;
    }
}
