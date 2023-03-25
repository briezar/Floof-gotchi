using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum UILayer
{
    Background,
    Main,
    Popup,
    Overlay,
}
public class UIManager : SingletonMonoBehaviour<UIManager>
{
    [SerializeField] private UnityEngine.AddressableAssets.AssetLabelReference[] _uiLabelPreload;
    [SerializeField] private Camera _uiCamera;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private Transform _mainCanvasGameRect, _worldCanvasGameRect;

    public Dictionary<UILayer, Transform> UILayers { get; private set; } = new();
    public CanvasCameraUI TopUI => _currentUIs.GetLast();
    public Camera UICamera => _uiCamera;
    public static bool IsInteractable => Instance._blockCount <= 0;

    private List<CanvasCameraUI> _currentUIs = new();
    private Dictionary<string, BaseUI> _preloadedUIs = new();

    private int _blockCount = 0;

    private void Awake()
    {
        var sampleLayer = _mainCanvasGameRect.GetChild(0);
        var layerNames = Enum.GetNames(typeof(UILayer));

        for (var i = 0; i < layerNames.Length; i++)
        {
            var newLayer = Transform.Instantiate(sampleLayer, _mainCanvasGameRect);
            newLayer.name = "Layer" + layerNames[i];
            UILayers.Add((UILayer)i, newLayer);
        }
        sampleLayer.DestroyGameObject();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (TopUI != null)
            {
                TopUI.OnBack();
            }
        }
    }

    public Coroutine PreloadUIsRoutine(Action<float> actionPercentComplete)
    {
        return AssetManager.PreloadAssetLabelRef<BaseUI>(_uiLabelPreload, (value) => { _preloadedUIs = value; }, actionPercentComplete);
    }

    private static string GetAddressUI<T>()
    {
        return "UI/" + typeof(T).Name + ".prefab";
    }

    private static T GetPreloadedUI<T>() where T : BaseUI
    {
        if (Instance._preloadedUIs.TryGetValue(typeof(T).Name, out var ui))
        {
            return (T)ui;
        }
        return null;
    }

    private static T GetInstantiatedUI<T>() where T : CanvasCameraUI
    {
        var ui = (T)Instance._currentUIs.Find((ui) => ui.GetType() == typeof(T));
        return ui;
    }

    public static void ShowAsyncWorldUI<T>(Action<T> onComplete = null) where T : WorldCameraUI
    {
        Instance.ShowAsyncWorldCamera<T>(onComplete);
    }

    public static void ShowAsyncUI<T>(Action<T> onComplete = null) where T : CanvasCameraUI
    {
        Instance.ShowAsync<T>(UILayer.Main, onComplete, true);
    }

    public static void ShowAsyncPopup<T>(Action<T> onComplete = null, bool singleInstance = false) where T : CanvasCameraUI
    {
        Instance.ShowAsync<T>(UILayer.Popup, onComplete, singleInstance);
    }

    public void ShowAsync<T>(UILayer uiLayer, Action<T> onComplete, bool singleInstance = true) where T : CanvasCameraUI
    {
        T ui = null;
        bool instantiate = !singleInstance;

        if (singleInstance)
        {
            ui = GetPreloadedUI<T>();
            if (ui == null) { instantiate = true; }
        }

        if (instantiate)
        {
            AssetManager.InstantiateAsync<T>(GetAddressUI<T>(), UILayers[uiLayer], OnComplete);
            // ui = GameObject.Instantiate<T>((T)_preloadedUIs[typeof(T).Name], UILayers[uILayer]);
        }
        else
        {
            OnComplete(ui);
        }

        void OnComplete(T targetUI)
        {
            Show(targetUI).SetLayer(uiLayer);
            onComplete?.Invoke(targetUI);
        }
    }

    private void ShowAsyncWorldCamera<T>(Action<T> onComplete) where T : WorldCameraUI
    {
        T ui = null;
        ui = GetPreloadedUI<T>();
        if (ui == null)
        {
            AssetManager.InstantiateAsync<T>(GetAddressUI<T>(), _worldCanvasGameRect, onComplete);
        }
        else
        {
            onComplete?.Invoke(ui);
        }
    }

    /// <summary>
    /// Only use when UI is preloaded!
    /// </summary>
    public static T ShowUI<T>(UILayer uILayer = UILayer.Main) where T : CanvasCameraUI
    {
        var name = typeof(T).Name;
        var ui = GetInstantiatedUI<T>();

        if (ui == null)
        {
            ui = GetPreloadedUI<T>();
            if (ui == null)
            {
                Debug.LogError(name + " is not preloaded!");
                return null;
            }
            ui = GameObject.Instantiate<T>(ui, Instance.UILayers[uILayer]);
            Instance._currentUIs.Add(ui);

        }

        return Instance.Show(ui);
    }

    public static T ShowWorldUI<T>() where T : WorldCameraUI
    {
        var name = typeof(T).Name;
        var ui = GetPreloadedUI<T>();
        if (ui == null)
        {
            Debug.LogError(name + " is not preloaded!");
            return null;
        }
        ui = GameObject.Instantiate<T>(ui, Instance._worldCanvasGameRect);
        return ui;
    }

    private T Show<T>(T ui) where T : CanvasCameraUI
    {
        _currentUIs.Remove(ui);
        _currentUIs.Add(ui);

        ui.transform.SetAsLastSibling();
        ui.gameObject.SetActive(true);

        ui.OnShow();

        return ui;
    }

    public static void HideLowerUI()
    {
        var count = Instance._currentUIs.Count;
        if (count <= 1) { return; }

        var index = count - 2;
        var ui = Instance._currentUIs[index];
        HideUI(ui);
    }

    public static void HideUI<T>() where T : CanvasCameraUI
    {
        var ui = GetInstantiatedUI<T>();
        if (ui == null) { return; }

        HideUI(ui);
    }
    public static void HideUI(CanvasCameraUI ui)
    {
        if (!ui.gameObject.activeInHierarchy) { return; }

        ui.OnHide();
        ui.gameObject.SetActive(false);
    }

    public void ShowNextUI(bool hideTopUI = true)
    {
        if (_currentUIs.Count <= 1) { return; }

        var lastTopUI = TopUI;

        _currentUIs.Remove(lastTopUI);
        _currentUIs.Insert(0, lastTopUI);

        if (hideTopUI)
        {
            HideUI(lastTopUI);
        }
        Show(TopUI);
    }

    public static void ReleaseUI<T>(bool releaseMemory = false) where T : CanvasCameraUI
    {
        var ui = GetInstantiatedUI<T>();
        if (releaseMemory)
        {
            if (ui == null)
            {
                ui = GetPreloadedUI<T>();
            }
            else
            {
                releaseMemory = false;
            }
        }
        else
        {
            if (ui == null) { return; }
        }

        ReleaseUI(ui, releaseMemory);
    }
    public static void ReleaseUI(CanvasCameraUI ui, bool releaseMemory = false)
    {
        Instance._currentUIs.Remove(ui);
        ui.DestroyGameObject(releaseMemory);
    }

    public static void SetInteractable(bool interactable, bool force = false)
    {
        if (force)
        {
            Instance._eventSystem.gameObject.SetActive(interactable);
            return;
        }

        if (interactable)
        {
            Instance._blockCount--;
            if (Instance._blockCount <= 0)
            {
                Instance._blockCount = 0;
                Instance._eventSystem.gameObject.SetActive(true);
            }
            return;
        }

        Instance._eventSystem.gameObject.SetActive(false);
        Instance._blockCount++;

    }
}
